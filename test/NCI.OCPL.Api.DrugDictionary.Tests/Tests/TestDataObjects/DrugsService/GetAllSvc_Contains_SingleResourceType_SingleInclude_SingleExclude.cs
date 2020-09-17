using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Test scenario
    ///     Match Begins With
    ///     20 results
    ///     Single DrugResourceType values.
    ///     Include single TermNameType values.
    ///     Exclude one TermNameType.
    /// </summary>
    public class GetAllSvc_Contains_SingleResourceType_SingleInclude_SingleExclude : BaseGetAllSvcRequestScenario
    {
        public override int From => 55;

        public override int Size => 20;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] { DrugResourceType.DrugTerm };

        public override TermNameType[] IncludeNameTypes => new TermNameType[] { TermNameType.USBrandName };

        public override TermNameType[] ExcludeNameTypes => new TermNameType[] { TermNameType.Synonym };

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""from"": 55,
            ""size"": 20,
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
                    ""must"": [
                        { ""terms"": { ""type"": [ ""DrugTerm"" ] } },
                        { ""terms"": { ""term_name_type"": [ ""USBrandName"" ] }
                        }
                    ],
                    ""must_not"": [
                        { ""terms"": { ""term_name_type"": [ ""Synonym"" ] } }
                    ]
                }
            }
        }
        ");

    }
}