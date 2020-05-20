using Newtonsoft.Json.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    public abstract class BaseAutosuggestServiceScenario
    {
        /// <summary>
        /// Gets the expected response data object.  Typically a JSON string
        /// wrapped in a call to JObject.Parse().
        /// </summary>
        public abstract JObject ExpectedData { get; }

        /// <summary>
        /// The text to search for.
        /// </summary>
        public abstract string SearchText { get; }

        /// <summary>
        /// The type of match (Begins vs Contains) being tested.
        /// </summary>
        public abstract MatchType MatchType { get; }

        /// <summary>
        /// Contains the maximum number of suggestions the controller should return.
        /// </summary>
        public abstract int Size { get; }

        /// <summary>
        /// Array of DrugResourceTypes to include.
        /// </summary>
        public abstract DrugResourceType[] IncludeResourceTypes { get; }

        /// <summary>
        /// Array of term name types to include.
        /// </summary>
        public abstract TermNameType[] IncludeNameTypes { get; }

        /// <summary>
        /// Array of term name types to exclude.
        /// </summary>
        public abstract TermNameType[] ExcludeNameTypes { get; }

    }
}