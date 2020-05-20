using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Test scenario
    ///     Match Begins With
    ///     5 results
    ///     Multiple DrugResourceType values.
    ///     Include multiple TermNameType values.
    ///     No TermNameType values to exclude.
    /// </summary>
    public class AutosuggestSvc_Begins_MultipleResourceTypes_MultipleIncludes : BaseAutosuggestServiceScenario
    {
        public override string SearchText => "bevaciz";

        public override MatchType MatchType => MatchType.Begins;

        public override int Size => 5;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] {DrugResourceType.DrugAlias, DrugResourceType.DrugTerm};

        public override TermNameType[] IncludeNameTypes => new TermNameType[] { TermNameType.USBrandName, TermNameType.Synonym };

        public override TermNameType[] ExcludeNameTypes => new TermNameType[0];

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""query"": {
                ""bool"": {
                    ""must"": [
                        { ""prefix"": { ""name"": { ""value"": ""bevaciz"" }} },
                        { ""terms"": { ""type"": [ ""DrugAlias"", ""DrugTerm"" ] } },
                        { ""terms"": { ""term_name_type"": [ ""USBrandName"", ""Synonym"" ] } }
                    ]
                }
            },
            ""sort"": [ { ""name"": {} } ],
            ""size"": 5,
            ""_source"": { ""includes"": [ ""term_id"", ""name"" ] }
        }
        ");

    }
}