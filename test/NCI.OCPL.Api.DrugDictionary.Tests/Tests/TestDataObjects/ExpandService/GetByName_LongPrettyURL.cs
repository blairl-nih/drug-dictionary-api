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
    public class GetByName_LongPrettyURL : BaseGetByNameSvcRequestScenario
    {
        public override string   PrettyUrlName => "adenoviral-transduced-hil-12-expressing-autologous-dendritic-cells-inxn-3001-plus-activator-ligand-inxn-1001";

        public override JObject ExpectedData => JObject.Parse(@"
        {
            ""sort"": [ { ""name"": {} } ],
            ""query"": {
                ""bool"": {
                    ""must"": [
                        { ""term"": { ""pretty_url_name"": { ""value"": ""adenoviral-transduced-hil-12-expressing-autologous-dendritic-cells-inxn-3001-plus-activator-ligand-inxn-1001"" } } },
                        { ""term"": { ""type"": { ""value"": ""DrugTerm"" } } }
                    ]
                }
            }
        }
        ");

    }}