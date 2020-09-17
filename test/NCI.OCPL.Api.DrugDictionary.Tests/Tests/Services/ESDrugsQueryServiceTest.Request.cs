using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Elasticsearch.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using Newtonsoft.Json.Linq;
using Xunit;

using NCI.OCPL.Api.Common.Testing;
using NCI.OCPL.Api.DrugDictionary.Models;
using NCI.OCPL.Api.DrugDictionary.Services;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    ///  Tests to verify the structure of requests to Elasticsearch.
    /// </summary>
    public partial class ESDrugsQueryServiceTest
    {
        public static IEnumerable<object[]> ExpandRequestScenarios = new[]
        {
            new object[] { new ExpandSvc_Begins_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new ExpandSvc_Begins_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new ExpandSvc_Begins_SingleResourceType_SingleInclude_SingleExclude() },
            new object[] { new ExpandSvc_Contains_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new ExpandSvc_Contains_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new ExpandSvc_Contains_SingleResourceType_SingleInclude_SingleExclude() }
        };

        /// <summary>
        ///  Verify structure of the request for Expand.
        /// </summary>
        [Theory, MemberData(nameof(ExpandRequestScenarios))]
        public async void Expand_TestRequestSetup(BaseExpandSvcRequestScenario data)
        {
            Uri esURI = null;
            string esContentType = String.Empty;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.

            JObject requestBody = null;

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.SearchResponse<IDrugResource>>((req, res) =>
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

            ESDrugsQueryService query = new ESDrugsQueryService(client, clientOptions, new NullLogger<ESDrugsQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            DrugTermResults result = await query.Expand(data.Letter, data.Size, data.From,
                data.IncludeResourceTypes, data.IncludeNameTypes, data.ExcludeNameTypes
                );

            Assert.Equal("/drugv1/terms/_search", esURI.AbsolutePath);
            Assert.Equal("application/json", esContentType);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.Equal(data.ExpectedData, requestBody, new JTokenEqualityComparer());
        }

        public static IEnumerable<object[]> GetAllRequestScenarios = new[]
        {
            new object[] { new GetAllSvc_Begins_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new GetAllSvc_Begins_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new GetAllSvc_Begins_SingleResourceType_SingleInclude_SingleExclude() },
            new object[] { new GetAllSvc_Contains_MultipleResourceTypes_MultipleIncludes() },
            new object[] { new GetAllSvc_Contains_MultipleResourceTypes_No_Include_MultipleExcludes() },
            new object[] { new GetAllSvc_Contains_SingleResourceType_SingleInclude_SingleExclude() }
        };

        /// <summary>
        ///  Verify structure of the request for GetAll.
        /// </summary>
        [Theory, MemberData(nameof(GetAllRequestScenarios))]
        public async void GetAll_TestRequestSetup(BaseGetAllSvcRequestScenario data)
        {
            Uri esURI = null;
            string esContentType = String.Empty;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.

            JObject requestBody = null;

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.SearchResponse<IDrugResource>>((req, res) =>
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

            ESDrugsQueryService query = new ESDrugsQueryService(client, clientOptions, new NullLogger<ESDrugsQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            DrugTermResults result = await query.GetAll(data.Size, data.From,
                data.IncludeResourceTypes, data.IncludeNameTypes, data.ExcludeNameTypes
                );

            Assert.Equal("/drugv1/terms/_search", esURI.AbsolutePath);
            Assert.Equal("application/json", esContentType);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.Equal(data.ExpectedData, requestBody, new JTokenEqualityComparer());
        }

        public static IEnumerable<object[]> GetByNameRequestScenarios = new[]
        {
            new object[] { new ExpandSvc_Olaparib() },
            new object[] { new ExpandSvc_WithDashes() },
            new object[] { new GetByName_LongPrettyURL() }
        };

        /// <summary>
        ///  Verify structure of the request for GetAll.
        /// </summary>
        [Theory, MemberData(nameof(GetByNameRequestScenarios))]
        public async void GetByName_TestRequestSetup(BaseGetByNameSvcRequestScenario data)
        {
            Uri esURI = null;
            string esContentType = String.Empty;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.

            JObject requestBody = null;

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.SearchResponse<DrugTerm>>((req, res) =>
            {
                // We don't really care about the response for this test.
                res.Stream = MockSingleTermResponse;
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

            ESDrugsQueryService query = new ESDrugsQueryService(client, clientOptions, new NullLogger<ESDrugsQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            DrugTerm result = await query.GetByName(data.PrettyUrlName);

            Assert.Equal("/drugv1/terms/_search", esURI.AbsolutePath);
            Assert.Equal("application/json", esContentType);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.Equal(data.ExpectedData, requestBody, new JTokenEqualityComparer());
        }

        public static IEnumerable<object[]> SearchRequestScenarios = new[]
        {
            new object[] { new SearchSvc_Begins_Olaparib() },
            new object[] { new SearchSvc_Contains_Cetuximab() },
            new object[] { new SearchSvc_ZeroOffset_Trametinib() },
            new object[] { new SearchSvc_Contains_LongName() },
            new object[] { new SearchSvc_Begins_LongName() },
            new object[] { new SearchSvc_Contains_Paclitaxel() }
        };

        /// <summary>
        ///  Verify structure of the request for Search.
        /// </summary>
        [Theory, MemberData(nameof(SearchRequestScenarios))]
        public async void Search_TestRequestSetup(BaseSearchSvcRequestScenario data)
        {
            Uri esURI = null;
            string esContentType = String.Empty;
            HttpMethod esMethod = HttpMethod.DELETE; // Basically, something other than the expected value.

            JObject requestBody = null;

            ElasticsearchInterceptingConnection conn = new ElasticsearchInterceptingConnection();
            conn.RegisterRequestHandlerForType<Nest.SearchResponse<DrugTerm>>((req, res) =>
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

            ESDrugsQueryService query = new ESDrugsQueryService(client, clientOptions, new NullLogger<ESDrugsQueryService>());

            // We don't really care that this returns anything (for this test), only that the intercepting connection
            // sets up the request correctly.
            DrugTermResults result = await query.Search(data.SearchText, data.MatchType, data.Size, data.From);

            Assert.Equal("/drugv1/terms/_search", esURI.AbsolutePath);
            Assert.Equal("application/json", esContentType);
            Assert.Equal(HttpMethod.POST, esMethod);
            Assert.Equal(data.ExpectedData, requestBody, new JTokenEqualityComparer());
        }

        private Stream MockSingleTermResponse
        {
            get
            {
                string res = @"
{
    ""took"": 2,
    ""timed_out"": false,
    ""_shards"": {
        ""total"": 1,
        ""successful"": 1,
        ""skipped"": 0,
        ""failed"": 0
    },
    ""hits"": {
        ""total"": 1,
        ""max_score"": null,
        ""hits"": [
            {
                ""_index"": ""drugv1"",
                ""_type"": ""terms"",
                ""_id"": ""37780"",
                ""_score"": null,
                ""_source"": {
                    ""term_id"": ""37780"",
                    ""name"": ""iodinated contrast dye"",
                    ""first_letter"": ""i"",
                    ""type"": ""DrugTerm"",
                    ""term_name_type"": ""PreferredName"",
                    ""pretty_url_name"": ""iodinated-contrast-agent"",
                    ""aliases"": [
                        {
                            ""type"": ""Synonym"",
                            ""name"": ""contrast dye, iodinated""
                        }
                    ],
                    ""definition"": {
                        ""text"": ""A contrast agent containing an iodine-based dye used in many diagnostic imaging examinations, including computed tomography, angiography, and myelography. Check for active clinical trials using this agent. (NCI Thesaurus)"",
                        ""html"": ""A contrast agent containing an iodine-based dye used in many diagnostic imaging examinations, including computed tomography, angiography, and myelography. Check for <a ref=\""https://www.cancer.gov/about-cancer/treatment/clinical-trials/intervention/C28500\"">active clinical trials</a> using this agent. (<a ref=\""https://ncit.nci.nih.gov/ncitbrowser/ConceptReport.jsp?dictionary=NCI%20Thesaurus&code=C28500\"">NCI Thesaurus</a>)""
                    },
                    ""nci_concept_id"": ""C28500"",
                    ""nci_concept_name"": ""Iodinated Contrast Agent""
                },
                ""sort"": [ ""iodinated contrast dye"" ]
            }
        ]
    }
}
                ";

                byte[] byteArray = Encoding.UTF8.GetBytes(res);
                return new MemoryStream(byteArray);
            }
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
                .Returns(new DrugDictionaryAPIOptions()
                {
                    AliasName = "drugv1"
                }
            );

            return clientOptions.Object;
        }
    }
}

