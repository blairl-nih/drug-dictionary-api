using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.DrugDictionary.Models;
using System;

namespace NCI.OCPL.Api.DrugDictionary.Services
{
    /// <summary>
    /// Elasticsearch implementation of the service for retrieveing suggestions for
    /// GlossaryTerm objects.
    /// </summary>
    public class ESAutosuggestQueryService : IAutosuggestQueryService
    {

        /// <summary>
        /// The elasticsearch client
        /// </summary>
        private IElasticClient _elasticClient;

        /// <summary>
        /// The API options.
        /// </summary>
        protected readonly DrugDictionaryAPIOptions _apiOptions;

        /// <summary>
        /// A logger to use for logging
        /// </summary>
        private readonly ILogger<ESAutosuggestQueryService> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ESAutosuggestQueryService(IElasticClient client,
            IOptions<DrugDictionaryAPIOptions> apiOptionsAccessor,
            ILogger<ESAutosuggestQueryService> logger)
        {
            _elasticClient = client;
            _apiOptions = apiOptionsAccessor.Value;
            _logger = logger;
        }

        /// <summary>
        /// Search for Terms based on the search criteria.
        /// </summary>
        /// <param name="searchText">The text to search for.</param>
        /// <param name="matchType">Set to true to allow search to find terms which contain the query string instead of explicitly starting with it.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>An array of Suggestion objects</returns>
        public async Task<Suggestion[]> GetSuggestions(string searchText, MatchType matchType, int size,
            DrugResourceType[] includeResourceTypes,
                TermNameType[] includeNameTypes,
                TermNameType[] excludeNameTypes
        )
        {
            // Set up the SearchRequest to send to elasticsearch.
            Indices index = Indices.Index(new string[] { this._apiOptions.AliasName });
            Types types = Types.Type(new string[] { "terms" });

            ISearchResponse<Suggestion> response = null;

            try
            {
                SearchRequest request;
                switch (matchType)
                {
                    default:
                    case MatchType.Begins:
                        request = BuildBeginRequest(index, types, searchText, size, includeResourceTypes, includeNameTypes, excludeNameTypes);
                        break;
                    case MatchType.Contains:
                        request = BuildContainsRequest(index, types, searchText, size, includeResourceTypes, includeNameTypes, excludeNameTypes);
                        break;
                }

                response = await _elasticClient.SearchAsync<Suggestion>(request);
            }
            catch (Exception ex)
            {
                string msg = "Could not search drug dictionary.";
                _logger.LogError($"Error searching index: '{this._apiOptions.AliasName}'.");
                _logger.LogError(ex, msg);
                throw new APIErrorException(500, msg);
            }

            if (!response.IsValid)
            {
                _logger.LogError($"Invalid response when searching for query '{searchText}', contains '{matchType}', size '{size}'.");
                throw new APIErrorException(500, "errors occured");
            }

            List<Suggestion> retVal = new List<Suggestion>(response.Documents);

            return retVal.ToArray();
        }

        /// <summary>
        /// Builds the SearchRequest for terms beginning with the search text.
        /// </summary>
        /// <param name="index">The index which will be searched against.</param>
        /// <param name="types">The list of document types to search.</param>
        /// <param name="query">The text to search for.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        private SearchRequest BuildBeginRequest(Indices index, Types types, string query, int size,
                DrugResourceType[] includeResourceTypes,
                    TermNameType[] includeNameTypes,
                    TermNameType[] excludeNameTypes
        )
        {
            SearchRequest request = new SearchRequest(index, types)
            {
                Query = new BoolQuery
                {
                    Must = new QueryContainer[]
                    {
                        new PrefixQuery {Field = "name", Value = query },
                        new TermsQuery { Field = "type", Terms = includeResourceTypes.Select(p => p.ToString()) },
                        new TermsQuery { Field = "term_name_type", Terms = includeNameTypes.Select(p => p.ToString()) }
                    },
                    MustNot = new QueryContainer[]
                    {
                        new TermsQuery { Field = "term_name_type", Terms = excludeNameTypes.Select(p => p.ToString()) }
                    },
                    Filter = new QueryContainer[]
                    {
                        new ScriptQuery
                        {
                            Inline = $"doc['name'].value.length() <= {_apiOptions.Autosuggest.MaxSuggestionLength}",
                            Lang = "painless"
                        }
                    }
                }
                ,
                Sort = new List<ISort>
                {
                    new SortField { Field = "name" }
                },
                Source = new SourceFilter
                {
                    Includes = new string[]{"term_id", "name"}
                },
                Size = size
            };

            return request;
        }

        /// <summary>
        /// Builds the SearchRequest for terms containing with the search text.
        /// </summary>
        /// <param name="index">The index which will be searched against.</param>
        /// <param name="types">The list of document types to search.</param>
        /// <param name="query">The text to search for.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        private SearchRequest BuildContainsRequest(Indices index, Types types, string query, int size,
                DrugResourceType[] includeResourceTypes,
                    TermNameType[] includeNameTypes,
                    TermNameType[] excludeNameTypes
        )
        {
            SearchRequest request = new SearchRequest(index, types)
            {
                Query = new BoolQuery
                {
                    Must = new QueryContainer[]
                    {
                        new MatchPhraseQuery { Field = "name._autocomplete", Query = query.ToString() },
                        new MatchQuery { Field = "name._contain", Query = query.ToString() },
                        new TermsQuery { Field = "type", Terms = includeResourceTypes.Select(p => p.ToString()) },
                        new TermsQuery {Field = "term_name_type", Terms = includeNameTypes.Select(p => p.ToString()) }
                    },
                    MustNot = new QueryContainer[]
                    {
                        new PrefixQuery { Field = "name", Value = query },
                        new TermsQuery { Field = "term_name_type", Terms = excludeNameTypes.Select(p => p.ToString())}
                    },
                    Filter = new QueryContainer[]
                    {
                        new ScriptQuery
                        {
                            Inline = $"doc['name'].value.length() <= {_apiOptions.Autosuggest.MaxSuggestionLength}",
                            Lang = "painless"
                        }
                    }
                }
                ,
                Sort = new List<ISort>
                {
                    new SortField { Field = "name" }
                },
                Source = new SourceFilter
                {
                    Includes = new string[] { "term_id", "name" }
                },
                Size = size
            };

            return request;
        }

    }
}