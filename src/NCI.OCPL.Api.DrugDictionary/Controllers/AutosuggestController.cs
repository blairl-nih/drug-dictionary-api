using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NCI.OCPL.Api.Common;
using NCI.OCPL.Api.DrugDictionary.Models;

namespace NCI.OCPL.Api.DrugDictionary.Controllers
{
    /// <summary>
    /// Controller for routes used when autosuggesting
    /// multiple Terms.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AutosuggestController : Controller
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Service instance for performing queries.
        /// </summary>
        private readonly IAutosuggestQueryService _autosuggestQueryService;

        /// <summary>
        /// Convenience list of all drug resource types, grouped in one place.
        /// </summary>
        private readonly DrugResourceType[] ALL_DRUG_RESOURCE_TYPES = (DrugResourceType[])Enum.GetValues(typeof(DrugResourceType));

        /// <summary>
        /// Convenience list of all term name types, grouped in one place.
        /// </summary>
        private readonly TermNameType[] ALL_TERM_NAME_TYPES = (TermNameType[])Enum.GetValues(typeof(TermNameType));

        /// <summary>
        /// Constructor.
        /// </summary>
        public AutosuggestController(ILogger<AutosuggestController> logger, IAutosuggestQueryService service)
        {
            _logger = logger;
            _autosuggestQueryService = service;
        }

        /// <summary>
        /// Searches for drug dictionary terms with names matching the query text.
        /// </summary>
        /// <param name="searchText">Text to match against</param>
        /// <param name="matchType">Should the search match items beginning with the search text, or containing it?</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>A Suggestion result containing the desired records.</returns>
        [HttpGet("")]
        public async Task<Suggestion[]> GetSuggestions(
            [FromQuery] string searchText,
            [FromQuery] MatchType matchType = MatchType.Begins, [FromQuery] int size = 20,
            [FromQuery] DrugResourceType[] includeResourceTypes = null,
            [FromQuery] TermNameType[] includeNameTypes = null,
            [FromQuery] TermNameType[] excludeNameTypes = null
            )
        {
            if(string.IsNullOrWhiteSpace(searchText))
            {
                throw new APIErrorException(400, "You must specify a search string.");
            }

            if (!Enum.IsDefined(typeof(MatchType), matchType))
                throw new APIErrorException(400, "The `matchType` parameter must be either 'Begins' or 'Contains'.");

            if (size <= 0)
                size = 20;

            // If Resource types to retrieve aren't specified, get all.
            if (includeResourceTypes == null || includeResourceTypes.Length == 0)
                includeResourceTypes = ALL_DRUG_RESOURCE_TYPES;

            // If term name types to retrieve aren't specified, get all.
            if (includeNameTypes == null || includeNameTypes.Length == 0)
                includeNameTypes = ALL_TERM_NAME_TYPES;

            // If term name types to exclude aren't specified, exclude none.
            if (excludeNameTypes == null)
                excludeNameTypes = new TermNameType[0];

            return await _autosuggestQueryService.GetSuggestions(searchText, matchType, size, includeResourceTypes, includeNameTypes, excludeNameTypes);
        }
    }
}