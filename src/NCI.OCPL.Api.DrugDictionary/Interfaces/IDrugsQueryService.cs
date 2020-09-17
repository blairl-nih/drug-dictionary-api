using System.Collections.Generic;
using System.Threading.Tasks;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Interface for the service for working with
    /// multiple Terms.
    /// </summary>
    public interface IDrugsQueryService
    {
        /// <summary>
        /// Retrieve a drug's definition based on its ID.
        /// <param name="id">The ID of the definition to retrieve.</param>
        /// <returns>A drug definitions object.</returns>
        /// </summary>
        Task<DrugTerm> GetById(long id);

        /// <summary>
        /// Retrieve a drug definitions based on its pretty URL name passed.
        /// <param name="prettyUrlName">The pretty url name to search for.</param>
        /// <returns>A drug definitions object.</returns>
        /// </summary>
        Task<DrugTerm> GetByName(string prettyUrlName);

        /// <summary>
        /// Retrieves a portion of the overall set of drug definitions for a given combination of dictionary, audience, and language.
        /// </summary>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="from">The offset into the overall set to use for the first record.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>A DrugTermResults object containing the desired records.</returns>
        Task<DrugTermResults> GetAll(int size, int from,
            DrugResourceType[] includeResourceTypes, TermNameType[] includeNameTypes, TermNameType[] excludeNameTypes
        );

        /// <summary>
        /// Search for drug definitions based on search criteria.
        /// <param name="query">The search query</param>
        /// <param name="matchType">Defines if the search should begin with or contain the key word</param>
        /// <param name="size">Defines the size of the search</param>
        /// <param name="from">Defines the Offset for search</param>
        /// <returns>A DrugTermResults object containing the desired records.</returns>
        /// </summary>
        Task<DrugTermResults> Search(string query, MatchType matchType, int size, int from);

        /// <summary>
        /// List all drug dictionary entries starting with the same first character.
        /// <param name="firstCharacter">The character to search for</param>
        /// <param name="size">Defines the size of the search</param>
        /// <param name="from">Defines the Offset for search</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>A DrugTermResults object containing the desired records.</returns>
        /// </summary>
        Task<DrugTermResults> Expand(char firstCharacter, int size, int from,
            DrugResourceType[] includeResourceTypes, TermNameType[] includeNameTypes, TermNameType[] excludeNameTypes
        );
    }
}