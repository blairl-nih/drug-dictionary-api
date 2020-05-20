using System.Collections.Generic;
using System.Threading.Tasks;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Interface for the service for working with
    /// auto suggestions
    /// </summary>
    public interface IAutosuggestQueryService
    {
        /// <summary>
        /// Retrieves a portion of the overall set of glossary terms for a given combination of dictionary, audience, and language.
        /// </summary>
        /// <param name="searchText">The search query</param>
        /// <param name="matchType">Should suggestions begin with the search text or contain it?.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>An array of Suggestion objects</returns>
        Task<Suggestion[]> GetSuggestions(string searchText, MatchType matchType, int size,
            DrugResourceType[] includeResourceTypes,
                TermNameType[] includeNameTypes,
                TermNameType[] excludeNameTypes
        );
    }

}