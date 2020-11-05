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
    public class AutosuggestSvc_Contains_MultipleResourceTypes_No_Include_MultipleExcludes : BaseAutosuggestServiceScenario
    {
        public override string SearchText => "ZFN ZFN-758";

        public override MatchType MatchType => MatchType.Contains;

        public override int Size => 3;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] { DrugResourceType.DrugAlias, DrugResourceType.DrugTerm };

        public override TermNameType[] IncludeNameTypes => new TermNameType[0];

        public override TermNameType[] ExcludeNameTypes => new TermNameType[] { TermNameType.USBrandName, TermNameType.ForeignBrandName };

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""query"": {
                ""bool"": {
                    ""must"": [
                        { ""match"": { ""name._autocomplete"": { ""query"": ""ZFN ZFN-758"", ""type"": ""phrase"" } } },
                        { ""match"": {""name._contain"": { ""query"": ""ZFN ZFN-758"" } } },
                        { ""terms"": { ""type"": [ ""DrugAlias"", ""DrugTerm"" ] } }
                    ],
                    ""must_not"": [
                        { ""prefix"": { ""name"": { ""value"": ""ZFN ZFN-758"" } } },
                        { ""terms"": { ""term_name_type"": [ ""USBrandName"", ""ForeignBrandName"" ] } }
                    ]
                }
            },
            ""size"": 3,
            ""_source"": { ""includes"": [ ""term_id"", ""name"" ] },
            ""sort"": [ { ""name"": {} } ]
        }
        ");

    }
}