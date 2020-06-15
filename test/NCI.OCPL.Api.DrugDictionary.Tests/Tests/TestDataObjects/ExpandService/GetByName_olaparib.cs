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
    public class ExpandSvc_Olaparib : BaseGetByNameSvcRequestScenario
    {
        public override string   PrettyUrlName => "olaparib";

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""sort"": [ { ""name"": {} } ],
            ""query"": {
                ""bool"": {
                    ""must"": [
                        { ""term"": { ""pretty_url_name"": { ""value"": ""olaparib"" } } },
                        { ""term"": { ""type"": { ""value"": ""DrugTerm"" } } }
                    ]
                }
            }
        }
        ");

    }}