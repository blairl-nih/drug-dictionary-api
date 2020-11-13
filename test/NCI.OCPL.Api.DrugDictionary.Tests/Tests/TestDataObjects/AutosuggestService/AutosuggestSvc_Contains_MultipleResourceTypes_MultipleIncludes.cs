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
    public class AutosuggestSvc_Contains_MultipleResourceTypes_MultipleIncludes : BaseAutosuggestServiceScenario
    {
        public override string SearchText => "bevaciz";

        public override MatchType MatchType => MatchType.Contains;

        public override int MaxSuggestionLength => 17;

        public override int Size => 5;

        public override DrugResourceType[] IncludeResourceTypes => new DrugResourceType[] {DrugResourceType.DrugAlias, DrugResourceType.DrugTerm};

        public override TermNameType[] IncludeNameTypes => new TermNameType[] { TermNameType.USBrandName, TermNameType.Synonym };

        public override TermNameType[] ExcludeNameTypes => new TermNameType[0];

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""query"": {
                ""bool"": {
                    ""must"": [
                        { ""match"": { ""name._autocomplete"": { ""query"": ""bevaciz"", ""type"": ""phrase"" } } },
                        { ""match"": {""name._contain"": { ""query"": ""bevaciz"" } } },
                        { ""terms"": { ""type"": [ ""DrugAlias"", ""DrugTerm"" ] } },
                        { ""terms"": { ""term_name_type"": [ ""USBrandName"", ""Synonym"" ] } }
                    ],
                    ""must_not"": [
                        { ""prefix"": { ""name"": { ""value"": ""bevaciz"" } } }
                    ],
                    ""filter"": [
                        {
                            ""script"": {
                                ""script"": {
                                    ""inline"": ""doc['name'].value.length() <= 17"",
                                    ""lang"": ""painless""
                                }
                            }
                        }
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