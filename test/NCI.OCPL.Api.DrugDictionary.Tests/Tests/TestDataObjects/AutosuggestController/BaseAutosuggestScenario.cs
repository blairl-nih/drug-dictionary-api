using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests.AutosuggestControllerTestData
{
    /// <summary>
    /// Base class for test data objects in mock responses from the autosuggest Service.
    /// </summary>
    public abstract class BaseAutosuggestControllerScenario
    {
        /// <summary>
        /// The type of match (Begins vs Contains) being tested.
        /// </summary>
        public abstract MatchType MatchType {get; }

        /// <summary>
        /// Contains the maximum number of suggestions the controller should return.
        /// NOTE: It is assumed that Size will never be smaller than the length of
        ///       the expected data.
        /// </summary>
        public abstract int Size {get; }

        /// <summary>
        /// Contains the data structure to return from the mock autosuggest service.
        /// </summary>
        public abstract Suggestion[] Data { get; }

        /// <summary>
        /// Gets the expected response from the controller.
        /// NOTE: It is assumed that the length of the expected data array
        ///       is never greater than the Size property.
        /// </summary>
        public abstract Suggestion[] ExpectedResult { get; }

    }
}