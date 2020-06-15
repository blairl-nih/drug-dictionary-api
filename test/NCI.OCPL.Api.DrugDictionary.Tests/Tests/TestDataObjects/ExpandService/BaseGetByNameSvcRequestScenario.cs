using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public abstract class BaseGetByNameSvcRequestScenario
    {
        /// <summary>
        /// Gets the expected response data object.  Typically a JSON string
        /// wrapped in a call to JObject.Parse().
        /// </summary>
        public abstract JObject ExpectedData { get; }

        /// <summary>
        /// Array of term name types to exclude.
        /// </summary>
        public abstract string PrettyUrlName { get; }
    }
}