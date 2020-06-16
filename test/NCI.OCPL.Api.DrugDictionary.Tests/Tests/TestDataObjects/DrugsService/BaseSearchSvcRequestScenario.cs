using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public abstract class BaseSearchSvcRequestScenario
    {
        /// <summary>
        /// Gets the expected response data object.  Typically a JSON string
        /// wrapped in a call to JObject.Parse().
        /// </summary>
        public abstract JObject ExpectedData { get; }

        /// <summary>
        /// The string being mock searched for.
        /// </summary>
        public abstract string SearchText { get; }

        /// <summary>
        /// The type of match (Begins vs Contains) being tested.
        /// </summary>
        public abstract MatchType MatchType { get; }

        /// <summary>
        /// The type of match (Begins vs Contains) being tested.
        /// </summary>
        public abstract int From { get; }

        /// <summary>
        /// Contains the maximum number of suggestions the controller should return.
        /// </summary>
        public abstract int Size { get; }

    }
}