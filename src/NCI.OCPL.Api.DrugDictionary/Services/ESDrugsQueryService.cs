using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.DrugDictionary.Models;

namespace NCI.OCPL.Api.DrugDictionary.Services
{

    /// <summary>
    /// Elasticsearch implementation of the service for retrieving multiple
    /// DrugTerm objects.
    /// </summary>
    public class ESDrugsQueryService : IDrugsQueryService
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
        private readonly ILogger<ESDrugsQueryService> _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ESDrugsQueryService(IElasticClient client, IOptions<DrugDictionaryAPIOptions> apiOptionsAccessor,
            ILogger<ESDrugsQueryService> logger)
        {
            _elasticClient = client;
            _apiOptions = apiOptionsAccessor.Value;
            _logger = logger;
        }

        /// <summary>
        /// Retrieve a drug's definition based on its ID.
        /// <param name="id">The ID of the definition to retrieve.</param>
        /// <returns>A drug definitions object.</returns>
        /// </summary>
        public async Task<DrugTerm> GetById(long id)
        {
            IGetResponse<DrugTerm> response = null;

            try
            {
                response = await _elasticClient.GetAsync<DrugTerm>(new DocumentPath<DrugTerm>(id),
                        g => g.Index(this._apiOptions.AliasName).Type("terms"));

            }
            catch (Exception ex)
            {
                String msg = $"Could not retrieve term id '{id}'.";
                _logger.LogError($"Error searching index: '{this._apiOptions.AliasName}'.");
                _logger.LogError(ex, msg);
                throw new APIErrorException(500, msg);
            }

            if (!response.IsValid)
            {
                String msg = $"Invalid response when retrieving id '{id}'.";
                _logger.LogError(msg);
                throw new APIErrorException(500, msg);
            }

            if (null == response.Source)
            {
                string msg = $"Not a valid ID '{id}'.";
                _logger.LogDebug(msg);
                throw new APIErrorException(404, msg);
            }

            return response.Source;
        }

        /// <summary>
        /// Retrieve a drug definitions based on its pretty URL name passed.
        /// <param name="prettyUrlName">The pretty url name to search for.</param>
        /// <returns>A drug definitions object.</returns>
        /// </summary>
        public async Task<DrugTerm> GetByName(string prettyUrlName)
        {
            // Set up the SearchRequest to send to elasticsearch.
            Indices index = Indices.Index(new string[] { this._apiOptions.AliasName });
            Types types = Types.Type(new string[] { "terms" });
            SearchRequest request = new SearchRequest(index, types)
            {
                Query = new TermQuery { Field = "pretty_url_name",  Value = prettyUrlName.ToString() } &&
                        new TermQuery { Field = "type",             Value = DrugResourceType.DrugTerm.ToString() }
                ,
                Sort = new List<ISort>
                {
                    new SortField { Field = "name" }
                }
            };

            ISearchResponse<DrugTerm> response = null;
            try
            {
                response = await _elasticClient.SearchAsync<DrugTerm>(request);
            }
            catch (Exception ex)
            {
                String msg = $"Could not search pretty URL name '{prettyUrlName}'.";
                _logger.LogError($"Error searching index: '{this._apiOptions.AliasName}'.");
                _logger.LogError(ex, msg);
                throw new APIErrorException(500, msg);
            }

            if (!response.IsValid)
            {
                String msg = $"Invalid response when searching for pretty URL name '{prettyUrlName}'.";
                _logger.LogError(msg);
                _logger.LogError(response.DebugInformation);
                throw new APIErrorException(500, "errors occured");
            }

            DrugTerm drugTerm;

            // If there is one or more terms in the response, then the search by pretty URL name was successful.
            if (response.Total > 0)
            {
                drugTerm = response.Documents.First();

                // If there is more than one term in the response, log a warning.
                if (response.Total > 1) {
                    string msg = $"Multiple matches for pretty URL name '{prettyUrlName}'.";
                    _logger.LogWarning(msg);
                }
            }
            else if (response.Total == 0)
            {
                string msg = $"No match for pretty URL name '{prettyUrlName}'.";
                _logger.LogDebug(msg);
                throw new APIErrorException(404, msg);
            }
            else
            {
                string msg = $"Incorrect response when searching for pretty URL name '{prettyUrlName}'.";
                _logger.LogError(msg);
                throw new APIErrorException(500, "Errors have occured.");
            }

            return drugTerm;
        }

        /// <summary>
        /// Get all drug dictionary entries.
        /// <param name="size">Defines the size of the search</param>
        /// <param name="from">Defines the Offset for search</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>A DrugTermResults object containing entries matching the desired criteria.</returns>
        /// </summary>
        public async Task<DrugTermResults> GetAll(int size, int from,
            DrugResourceType[] includeResourceTypes,
            TermNameType[] includeNameTypes,
            TermNameType[] excludeNameTypes)
        {
            // Elasticsearch knows how to figure out what the ElasticSearch name is for
            // a given field when given a PropertyInfo.
            Field[] fieldList = getFieldList()
                                    .Select(pi => new Field(pi))
                                    .ToArray();

            // Set up the SearchRequest to send to elasticsearch.
            Indices index = Indices.Index(new string[] { this._apiOptions.AliasName });
            Types types = Types.Type(new string[] { "terms" });
            SearchRequest request = new SearchRequest(index, types)
            {
                Query =
                    new TermsQuery { Field = "type", Terms = includeResourceTypes.Select(p => p.ToString()) } &&
                    new TermsQuery { Field = "term_name_type", Terms = includeNameTypes.Select(p => p.ToString()) } &&
                    !new TermsQuery { Field = "term_name_type", Terms = excludeNameTypes.Select(p => p.ToString()) }
                ,
                Sort = new List<ISort>
                {
                    new SortField { Field = "name" }
                },
                Size = size,
                From = from,
                Source = new SourceFilter
                {
                    Includes = fieldList
                }
            };

            ISearchResponse<IDrugResource> response = null;
            try
            {
                response = await _elasticClient.SearchAsync<IDrugResource>(request);
            }
            catch (Exception ex)
            {
                String msg = $"Could not search size '{size}', from '{from}'.";
                _logger.LogError($"Error searching index: '{this._apiOptions.AliasName}'.");
                _logger.LogError(msg, ex);
                throw new APIErrorException(500, msg);
            }

            if (!response.IsValid)
            {
                String msg = $"Invalid response when searching for size '{size}', from '{from}'.";
                _logger.LogError(msg);
                throw new APIErrorException(500, "errors occured");
            }

            DrugTermResults drugTermResults = new DrugTermResults();

            if (response.Total > 0)
            {
                drugTermResults.Results = response.Documents.Select(res => (IDrugResource)res).ToArray();
            }
            else if (response.Total == 0)
            {
                // Create an empty list.
                drugTermResults.Results = new IDrugResource[] { };
            }

            // Add the metadata for the returned results
            drugTermResults.Meta = new ResultsMetadata()
            {
                TotalResults = (int)response.Total,
                From = from
            };

            return drugTermResults;
        }

        /// <summary>
        /// Search for drug definitions based on search criteria.
        /// <param name="query">The search query</param>
        /// <param name="matchType">Defines if the search should begin with or contain the key word</param>
        /// <param name="size">Defines the size of the search</param>
        /// <param name="from">Defines the Offset for search</param>
        /// <returns>A DrugTermResults object containing the desired records.</returns>
        /// </summary>
        public async Task<DrugTermResults> Search(string query, MatchType matchType, int size, int from)
        {
            // Elasticsearch knows how to figure out what the ElasticSearch name is for
            // fields when given a PropertyInfo.
            Field[] fieldList = getFieldList()
                                    .Select(pi => new Field(pi))
                                    .ToArray();

            // Set up the SearchRequest to send to elasticsearch.
            Indices index = Indices.Index(new string[] { this._apiOptions.AliasName });
            Types types = Types.Type(new string[] { "terms" });
            SearchRequest request = new SearchRequest(index, types)
            {
                Query = (
                            (matchType == MatchType.Begins ?
                                (QueryBase)new PrefixQuery { Field = "name", Value = query } :
                                (QueryBase)new MatchQuery { Field = "name._contain", Query = query }
                            ) &&
                            new TermQuery { Field = "type", Value = DrugResourceType.DrugTerm.ToString() }
                        ) ||
                        new NestedQuery
                            {
                                Path = "aliases",
                                Query = (matchType == MatchType.Begins ?
                                        (QueryBase)new PrefixQuery { Field = "aliases.name", Value = query} :
                                        (QueryBase)new MatchQuery { Field = "aliases.name._contain", Query = query}
                                    )
                            }
                ,
                Sort = new List<ISort>
                {
                    new SortField { Field = "name" }
                },
                Size = size,
                From = from,
                Source = new SourceFilter
                {
                    Includes = fieldList
                }
            };

            ISearchResponse<DrugTerm> response = null;
            try
            {
                response = await _elasticClient.SearchAsync<DrugTerm>(request);
            }
            catch (Exception ex)
            {
                String msg = $"Could not search query '{query}', size '{size}', from '{from}'.";
                _logger.LogError($"Error searching index: '{this._apiOptions.AliasName}'.");
                _logger.LogError(msg, ex);
                throw new APIErrorException(500, msg);
            }

            if (!response.IsValid)
            {
                String msg = $"Invalid response when searching for query '{query}', size '{size}', from '{from}'.";
                _logger.LogError(msg);
                _logger.LogError(response.DebugInformation);
                throw new APIErrorException(500, "errors occured");
            }

            DrugTermResults searchResults = new DrugTermResults();

            if (response.Total > 0)
            {
                // Build the array of glossary terms for the returned results.
                List<DrugTerm> termResults = new List<DrugTerm>();
                foreach (DrugTerm res in response.Documents)
                {
                    termResults.Add(res);
                }

                searchResults.Results = termResults.ToArray();

                // Add the metadata for the returned results
                searchResults.Meta = new ResultsMetadata()
                {
                    TotalResults = (int)response.Total,
                    From = from
                };
            }
            else if (response.Total == 0)
            {
                // Add the defualt value of empty GlossaryTerm list.
                searchResults.Results = new DrugTerm[0];

                // Add the metadata for the returned results
                searchResults.Meta = new ResultsMetadata()
                {
                    TotalResults = (int)response.Total,
                    From = from
                };
            }

            return searchResults;
        }

        /// <summary>
        /// List all drug dictionary entries starting with the same first character.
        /// <param name="firstCharacter">The character to search for.</param>
        /// <param name="size">Defines the size of the search</param>
        /// <param name="from">Defines the Offset for search</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>A DrugTermResults object containing entries matching the desired criteria.</returns>
        /// </summary>
        public async Task<DrugTermResults> Expand(char firstCharacter, int size, int from,
            DrugResourceType[] includeResourceTypes,
            TermNameType[] includeNameTypes,
            TermNameType[] excludeNameTypes
        )
        {
            // Elasticsearch knows how to figure out what the ElasticSearch name is for
            // fields when given a PropertyInfo.
            Field[] fieldList = getFieldList()
                                    .Select(pi => new Field(pi))
                                    .ToArray();

            // Set up the SearchRequest to send to elasticsearch.
            Indices index = Indices.Index(new string[] { this._apiOptions.AliasName });
            Types types = Types.Type(new string[] { "terms" });
            SearchRequest request = new SearchRequest(index, types)
            {
                Query =
                    new TermQuery { Field = "first_letter",         Value = firstCharacter.ToString() } &&
                    new TermsQuery { Field = "type",                Terms = includeResourceTypes.Select(p => p.ToString())} &&
                    new TermsQuery { Field = "term_name_type",      Terms = includeNameTypes.Select(p => p.ToString())} &&
                    !new TermsQuery {Field = "term_name_type",      Terms = excludeNameTypes.Select(p => p.ToString())}
                ,
                Sort = new List<ISort>
                {
                    new SortField { Field = "name" }
                },
                Size = size,
                From = from,
                Source = new SourceFilter
                {
                    Includes = fieldList
                }
            };

            ISearchResponse<IDrugResource> response = null;
            try
            {
                response = await _elasticClient.SearchAsync<IDrugResource>(request);
            }
            catch (Exception ex)
            {
                String msg = $"Could not search character '{firstCharacter}', size '{size}', from '{from}'.";
                _logger.LogError($"Error searching index: '{this._apiOptions.AliasName}'.");
                _logger.LogError(msg, ex);
                throw new APIErrorException(500, msg);
            }

            if (!response.IsValid)
            {
                String msg = $"Invalid response when searching for character '{firstCharacter}', size '{size}', from '{from}'.";
                _logger.LogError(msg);
                throw new APIErrorException(500, "errors occured");
            }

            DrugTermResults drugTermResults = new DrugTermResults();

            if (response.Total > 0)
            {
                drugTermResults.Results = response.Documents.Select(res => (IDrugResource)res).ToArray();
            }
            else if (response.Total == 0)
            {
                // Add the defualt value of empty GlossaryTerm list.
                drugTermResults.Results = new DrugTerm[] { };
            }

            // Add the metadata for the returned results
            drugTermResults.Meta = new ResultsMetadata()
            {
                TotalResults = (int)response.Total,
                From = from
            };

            return drugTermResults;
        }

        /// <summary>
        /// Yields a collection of PropertyInfo objects representing the named fields passed into the API.
        /// </summary>
        /// <returns>An array of PropertyInfo objects representing the fields. NOTE: If a field is a value type then it
        /// must be returned as part of the object so that we do not get incorrect default values. Dotnet cannot actually change
        /// the shape of our object, or at least not without major trouble. So we will force the return here.</returns>
        private IEnumerable<PropertyInfo> getFieldList()
        {
            // We will use reflection to get a list of the properties from the DrugTerm and DrugAlias classes.
            // Combine the two lists, and return all
            PropertyInfo[] termProperties = typeof(DrugTerm).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] aliasProperties = typeof(DrugAlias).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var combinedList = termProperties.ToList();
            combinedList.AddRange(aliasProperties);

            // DrugTerm and DrugAlias are both implementations of IDrugResource.
            // Any property that exists in both should be the same field.
            // Ergo, we need to track which property names are being returned,
            // and skip any which have already been returned.

            // Track the properties returned so far.
            HashSet<string> propertiesReturned = new HashSet<string>();

            foreach (PropertyInfo property in combinedList)
            {
                if( propertiesReturned.Contains(property.Name.ToLower()))
                    continue;

                propertiesReturned.Add(property.Name.ToLower());

                yield return property;
            }
        }
    }
}