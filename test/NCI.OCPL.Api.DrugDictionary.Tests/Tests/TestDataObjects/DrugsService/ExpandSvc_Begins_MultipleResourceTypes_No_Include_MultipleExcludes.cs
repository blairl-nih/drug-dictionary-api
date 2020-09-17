using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Test scenario
    ///     Match Begins With
    ///     8 results
    ///     Multiple DrugResourceType values.
    ///     Don't list TermNameType values to include.
    ///     Exclude multiple TermNameType values.
    /// </summary>
    public class ExpandSvc_Begins_MultipleResourceTypes_No_Include_MultipleExcludes : BaseExpandSvcRequestScenario
    {
        public override char Letter => 'z';

        public override int From => 10;

        public override int Size => 8;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] { DrugResourceType.DrugAlias, DrugResourceType.DrugTerm };

        public override TermNameType[] IncludeNameTypes => new TermNameType[0];

        public override TermNameType[] ExcludeNameTypes => new TermNameType[] { TermNameType.CASRegistryName, TermNameType.ChemicalStructureName };

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""from"": 10,
            ""size"": 8,
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
                        { ""term"": { ""first_letter"": { ""value"": ""z"" } } },
                        { ""terms"": { ""type"": [ ""DrugAlias"", ""DrugTerm"" ] } }
                    ],
                    ""must_not"": [
                        { ""terms"": { ""term_name_type"": [ ""CASRegistryName"", ""ChemicalStructureName"" ] } }
                    ]
                }
            }
        }        ");

    }
}