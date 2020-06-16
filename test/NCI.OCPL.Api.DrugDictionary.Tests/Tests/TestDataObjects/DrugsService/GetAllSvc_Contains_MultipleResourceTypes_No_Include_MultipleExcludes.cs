using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Test scenario
    ///     Match Begins With
    ///     3 results
    ///     Multiple DrugResourceType values.
    ///     Don't list TermNameType values to include.
    ///     Exclude multiple TermNameType values.
    /// </summary>
    public class GetAllSvc_Contains_MultipleResourceTypes_No_Include_MultipleExcludes : BaseGetAllSvcRequestScenario
    {
        public override int From => 200;

        public override int Size => 3;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] { DrugResourceType.DrugAlias, DrugResourceType.DrugTerm };

        public override TermNameType[] IncludeNameTypes => new TermNameType[0];

        public override TermNameType[] ExcludeNameTypes => new TermNameType[] { TermNameType.USBrandName, TermNameType.ForeignBrandName };

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""from"": 200,
            ""size"": 3,
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
                    ""pretty_url_name""
                ]
            },
            ""sort"": [ { ""name"": {} } ],
            ""query"": {
                ""bool"": {
                    ""must"": [
                        { ""terms"": { ""type"": [ ""DrugAlias"", ""DrugTerm"" ] } }
                    ],
                    ""must_not"": [
                        { ""terms"": { ""term_name_type"": [ ""USBrandName"", ""ForeignBrandName"" ] } }
                    ]
                }
            }
        }
        ");

    }
}