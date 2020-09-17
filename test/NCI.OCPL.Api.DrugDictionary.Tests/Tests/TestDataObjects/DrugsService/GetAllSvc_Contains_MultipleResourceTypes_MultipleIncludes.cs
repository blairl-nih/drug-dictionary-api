using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Test scenario
    ///     Match Contains
    ///     5 results
    ///     Multiple DrugResourceType values.
    ///     Include multiple TermNameType values.
    ///     No TermNameType values to exclude.
    /// </summary>
    public class GetAllSvc_Contains_MultipleResourceTypes_MultipleIncludes : BaseGetAllSvcRequestScenario
    {
        public override int From => 100;

        public override int Size => 5;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] {DrugResourceType.DrugAlias, DrugResourceType.DrugTerm};

        public override TermNameType[] IncludeNameTypes => new TermNameType[] { TermNameType.USBrandName, TermNameType.Synonym };

        public override TermNameType[] ExcludeNameTypes => new TermNameType[0];

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""from"": 100,
            ""size"": 5,
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
                        { ""terms"": { ""type"": [ ""DrugAlias"", ""DrugTerm"" ] } },
                        { ""terms"": { ""term_name_type"": [ ""USBrandName"", ""Synonym"" ] } }
                    ]
                }
            }
        }
        ");

    }
}