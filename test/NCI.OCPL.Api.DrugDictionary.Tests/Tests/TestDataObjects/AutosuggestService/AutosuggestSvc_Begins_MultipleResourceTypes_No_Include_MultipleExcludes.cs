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
    public class AutosuggestSvc_Begins_MultipleResourceTypes_No_Include_MultipleExcludes : BaseAutosuggestServiceScenario
    {
        public override string SearchText => "zanoli";

        public override MatchType MatchType => MatchType.Begins;

        public override int Size => 8;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] { DrugResourceType.DrugAlias, DrugResourceType.DrugTerm };

        public override TermNameType[] IncludeNameTypes => new TermNameType[0];

        public override TermNameType[] ExcludeNameTypes => new TermNameType[] { TermNameType.CASRegistryName, TermNameType.ChemicalStructureName };

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""query"": {
                ""bool"": {
                    ""must"": [
                        { ""prefix"": { ""name"": { ""value"": ""zanoli"" } } },
                        { ""terms"": { ""type"": [ ""DrugAlias"", ""DrugTerm"" ] } }
                    ],
                    ""must_not"": [
                        { ""terms"": { ""term_name_type"": [ ""CASRegistryName"", ""ChemicalStructureName"" ] } }
                    ]
                }
            },
            ""size"": 8,
            ""sort"": [ { ""name"": {} } ],
            ""_source"": { ""includes"": [ ""term_id"", ""name"" ] }
        }
        ");

    }
}