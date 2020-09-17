using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class SearchSvc_ZeroOffset_Trametinib : BaseSearchSvcRequestScenario
    {
        public override string SearchText => "trametinib";

        public override MatchType MatchType => MatchType.Begins;

        public override int From => 0;

        public override int Size => 100;

        public override JObject ExpectedData => JObject.Parse(@"
{
    ""from"": 0,
    ""size"": 100,
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
                            { ""prefix"": { ""name"": { ""value"": ""trametinib"" } } },
                            { ""term"": { ""type"": { ""value"": ""DrugTerm"" } } }
                        ]
                    }
                },
                {
                    ""nested"": {
                        ""query"": { ""prefix"": { ""aliases.name"": { ""value"": ""trametinib"" } } },
                        ""path"": ""aliases""
                    }
                }
            ]
        }
    }
}        ");

    }
}