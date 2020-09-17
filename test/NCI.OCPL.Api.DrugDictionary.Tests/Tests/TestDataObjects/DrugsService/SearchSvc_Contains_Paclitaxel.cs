using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class SearchSvc_Contains_Paclitaxel : BaseSearchSvcRequestScenario
    {
        public override string SearchText => "paclitaxel";

        public override MatchType MatchType => MatchType.Contains;

        public override int From => 0;

        public override int Size => 200;

        public override JObject ExpectedData => JObject.Parse(@"
{
    ""from"": 0,
    ""size"": 200,
    ""_source"": {
        ""includes"": [
            ""aliases"",
            ""definition"",
            ""drug_info_summary_link"",
            ""nci_concept_id"",
            ""nci_concept_name"",
            ""term_id"",
            ""name"",
            ""first_letter"",
            ""type"",
            ""term_name_type"",
            ""pretty_url_name"",
            ""preferred_name""
        ]
    },
    ""sort"": [ { ""name"": {} } ],
    ""query"": {
        ""bool"": {
            ""should"": [
                {
                    ""bool"": {
                        ""must"": [
                            { ""match"": { ""name._contain"": { ""query"": ""paclitaxel"" } } },
                            { ""term"": { ""type"": { ""value"": ""DrugTerm"" } } }
                        ]
                    }
                },
                {
                    ""nested"": {
                        ""query"": { ""match"": { ""aliases.name._contain"": { ""query"": ""paclitaxel"" } } },
                        ""path"": ""aliases""
                    }
                }
            ]
        }
    }
}        ");

    }
}