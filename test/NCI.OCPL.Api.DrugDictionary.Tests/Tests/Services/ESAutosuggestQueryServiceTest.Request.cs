using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Testing;
using Moq;
using Nest;
using Newtonsoft.Json.Linq;
using Xunit;

using NCI.OCPL.Api.Common.Testing;
using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public partial class ESAutosuggestQueryServiceTest
    {
        public static IEnumerable<object[]> AutosuggestRequestScenarios = new[]
        {
            new object[] { new AutosuggestSvc_Begins_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new AutosuggestSvc_Contains_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new AutosuggestSvc_Begins_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new AutosuggestSvc_Contains_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new AutosuggestSvc_Begins_SingleResourceType_SingleInclude_SingleExclude() },
            new object[] { new AutosuggestSvc_Contains_SingleResourceType_SingleInclude_SingleExclude() }
        };

        /// <summary>
        /// Test to verify that Elasticsearch requests are being assembled as expected.
        /// </summary>
        [Theory, MemberData(nameof(AutosuggestRequestScenarios))]
        public async void GetSuggestions_TestRequestSetup(BaseAutosuggestServiceScenario data)
        {
            Uri esURI = null;
            string esContentType = String.Empty;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.

            JObject requestBody = null;

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.SearchResponse<Suggestion>>((req, res) =>
            {
                // We don't really care about the response for this test.
                res.Stream = MockEmptyResponse;
                res.StatusCode = 200;

                esURI = req.Uri;
                esContentType = req.ContentType;
                esMethod = req.Method;
                requestBody = conn.GetRequestPost(req);
            });

            // The URI does not matter, an InMemoryConnection never requests from the server.
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));

            var connectionSettings = new ConnectionSettings(pool, conn);
            IElasticClient client = new ElasticClient(connectionSettings);

            // Setup the mocked Options
            IOptions<DrugDictionaryAPIOptions> clientOptions = GetMockOptions();
            clientOptions.Value.Autosuggest.MaxSuggestionLength = data.MaxSuggestionLength;

            ESAutosuggestQueryService query = new ESAutosuggestQueryService(client, clientOptions, new NullLogger<ESAutosuggestQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            Suggestion[] result = await query.GetSuggestions(data.SearchText, data.MatchType, data.Size,
                data.IncludeResourceTypes, data.IncludeNameTypes, data.ExcludeNameTypes);

            Assert.Equal("/drugv1/terms/_search", esURI.AbsolutePath);
            Assert.Equal("application/json", esContentType);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.Equal(data.ExpectedData, requestBody, new JTokenEqualityComparer());
        }

        /// <summary>
        /// Simulates a "no results found" response from Elasticsearch so we
        /// have something for tests where we don't care about the response.
        /// </summary>
        private Stream MockEmptyResponse
        {
            get
            {
                string empty = @"
{
    ""took"": 223,
    ""timed_out"": false,
    ""_shards"": {
                ""total"": 1,
        ""successful"": 1,
        ""skipped"": 0,
        ""failed"": 0
    },
    ""hits"": {
                ""total"": 0,
        ""max_score"": null,
        ""hits"": []
    }
}";
                byte[] byteArray = Encoding.UTF8.GetBytes(empty);
                return new MemoryStream(byteArray);
            }
        }


        /// <summary>
        /// Mock Elasticsearch configuraiton options.
        /// </summary>
        private IOptions<DrugDictionaryAPIOptions> GetMockOptions()
        {
            Mock<IOptions<DrugDictionaryAPIOptions>> clientOptions = new Mock<IOptions<DrugDictionaryAPIOptions>>();
            clientOptions
                .SetupGet(opt => opt.Value)
                .Returns(new DrugDictionaryAPIOptions
                {
                    AliasName = "drugv1",
                    Autosuggest = new AutosuggestOptions
                    {
                        MaxSuggestionLength = 30
                    }
                }
            );

            return clientOptions.Object;
        }
    }
}