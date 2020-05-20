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
    public class AutosuggestSvc_Begins_SingleResourceType_SingleInclude_SingleExclude : BaseAutosuggestServiceScenario
    {
        public override string SearchText => "gadavi";

        public override MatchType MatchType => MatchType.Begins;

        public override int Size => 20;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] { DrugResourceType.DrugAlias };

        public override TermNameType[] IncludeNameTypes => new TermNameType[] { TermNameType.USBrandName };

        public override TermNameType[] ExcludeNameTypes => new TermNameType[] { TermNameType.Synonym };

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""query"": {
                ""bool"": {
                    ""must"": [
                        { ""prefix"": { ""name"": { ""value"": ""gadavi"" } } },
                        { ""terms"": { ""type"": [ ""DrugAlias"" ] } },
                        { ""terms"": { ""term_name_type"": [ ""USBrandName"" ] } }
                    ],
                    ""must_not"": [
                        { ""terms"": { ""term_name_type"": [ ""Synonym"" ] } }
                    ]
                }
            },
            ""sort"": [ { ""name"": {} } ],
            ""size"": 20,
            ""_source"": { ""includes"": [ ""term_id"", ""name"" ] }
        }
        ");

    }
}