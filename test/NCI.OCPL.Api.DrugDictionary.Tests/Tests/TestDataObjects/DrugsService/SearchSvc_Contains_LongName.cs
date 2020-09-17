using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public class SearchSvc_Contains_LongName : BaseSearchSvcRequestScenario
    {
        public override string SearchText => "adenoviral-transduced hIL-12-expressing autologous dendritic cells INXN-3001 plus activator ligand INXN-1001";

        public override MatchType MatchType => MatchType.Contains;

        public override int From => 100;

        public override int Size => 50;

        public override JObject ExpectedData => JObject.Parse(@"
{
    ""from"": 100,
    ""size"": 50,
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
                            { ""match"": { ""name._contain"": { ""query"": ""adenoviral-transduced hIL-12-expressing autologous dendritic cells INXN-3001 plus activator ligand INXN-1001"" } } },
                            { ""term"": { ""type"": { ""value"": ""DrugTerm"" } } }
                        ]
                    }
                },
                {
                    ""nested"": {
                        ""query"": { ""match"": { ""aliases.name._contain"": { ""query"": ""adenoviral-transduced hIL-12-expressing autologous dendritic cells INXN-3001 plus activator ligand INXN-1001"" } } },
                        ""path"": ""aliases""
                    }
                }
            ]
        }
    }
}        ");

    }
}