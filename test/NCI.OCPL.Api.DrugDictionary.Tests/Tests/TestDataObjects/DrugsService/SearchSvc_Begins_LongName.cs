using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class SearchSvc_Begins_LongName : BaseSearchSvcRequestScenario
    {
        public override string SearchText => "anaplastic lymphoma kinase inhibitor CH5424802";

        public override MatchType MatchType => MatchType.Begins;

        public override int From => 200;

        public override int Size => 100;

        public override JObject ExpectedData => JObject.Parse(@"
{
    ""from"": 200,
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
                            { ""prefix"": { ""name"": { ""value"": ""anaplastic lymphoma kinase inhibitor CH5424802"" } } },
                            { ""term"": { ""type"": { ""value"": ""DrugTerm"" } } }
                        ]
                    }
                },
                {
                    ""nested"": {
                        ""query"": { ""prefix"": { ""aliases.name"": { ""value"": ""anaplastic lymphoma kinase inhibitor CH5424802"" } } },
                        ""path"": ""aliases""
                    }
                }
            ]
        }
    }
}        ");

    }
}