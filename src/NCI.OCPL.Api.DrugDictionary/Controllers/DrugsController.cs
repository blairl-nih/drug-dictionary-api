using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using NCI.OCPL.Api.Common;
using System.Net;

namespace NCI.OCPL.Api.DrugDictionary.Controllers
{
    /// <summary>
    /// Controller for routes used when searching for or retrieving
    /// multiple Terms.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class DrugsController : Controller
    {
        /// <summary>
        /// The logger instance.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The query service to use.
        /// </summary>
        private readonly IDrugsQueryService _termsQueryService;

        /// <summary>
        /// Convenience list of all drug resource types, grouped in one place.
        /// </summary>
        private readonly DrugResourceType[] ALL_DRUG_RESOURCE_TYPES = (DrugResourceType[])Enum.GetValues(typeof(DrugResourceType));

        /// <summary>
        /// Convenience list of all term name types, grouped in one place.
        /// </summary>
        private readonly TermNameType[] ALL_TERM_NAME_TYPES = (TermNameType[])Enum.GetValues(typeof(TermNameType));

        /// <summary>
        /// Convenience list of default fields to request.
        /// This is the union of fields available from the DrugTerm and DrugAlias resource types.
        /// </summary>
        private readonly string[] DEFAULT_FIELD_NAMES = { "termId", "name", "firstLetter", "type", "termNameType", "prettyUrlName", "aliases", "definition", "drugInfoSummaryLink", "nciConceptId", "nciConceptName", "PreferredName" };

        /// <summary>
        /// Constructor.
        /// </summary>
        public DrugsController(ILogger<DrugsController> logger, IDrugsQueryService service)
        {
            _logger = logger;
            _termsQueryService = service;
        }

        /// <summary>
        /// List all drug dictionary entries starting with the same first character.
        /// </summary>
        /// <param name="character">The character to expand. Use # for non-alphabetic.</param>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="from">The offset into the overall set to use for the first record.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>A DrugTermResults object containing the desired records.</returns>
        [HttpGet("expand/{character}")]
        public async Task<DrugTermResults> Expand(char character,
            [FromQuery] int size = 100,
            [FromQuery] int from = 0,
            [FromQuery] DrugResourceType[] includeResourceTypes = null,
            [FromQuery] TermNameType[] includeNameTypes = null,
            [FromQuery] TermNameType[] excludeNameTypes = null
        )
        {
            if (size <= 0)
                size = 100;

            if (from < 0)
                from = 0;

            // If Resource types to retrieve aren't specified, get all.
            if (includeResourceTypes == null || includeResourceTypes.Length == 0)
                includeResourceTypes = ALL_DRUG_RESOURCE_TYPES;

            // If term name types to retrieve aren't specified, get all.
            if (includeNameTypes == null || includeNameTypes.Length == 0)
                includeNameTypes = ALL_TERM_NAME_TYPES;

            // If term name types to exclude aren't specified, exclude none.
            if (excludeNameTypes == null)
                excludeNameTypes = new TermNameType[0];

            DrugTermResults res = await _termsQueryService.Expand(character, size, from,
                includeResourceTypes, includeNameTypes, excludeNameTypes);

            return res;
        }

        /// <summary>
        /// Get a list of all terms (no search string).
        /// </summary>
        /// <param name="size">The number of records to retrieve.</param>
        /// <param name="from">The offset into the overall set to use for the first record.</param>
        /// <param name="includeResourceTypes">The DrugResourceTypes to include. Default: All</param>
        /// <param name="includeNameTypes">The name types to include. Default: All</param>
        /// <param name="excludeNameTypes">The name types to exclude. Default: All</param>
        /// <returns>A DrugTermResults object containing the desired records.</returns>
        [HttpGet("")]
        public async Task<DrugTermResults> GetAll(
            [FromQuery] int size = 100, [FromQuery] int from = 0,
            [FromQuery] DrugResourceType[] includeResourceTypes = null,
            [FromQuery] TermNameType[] includeNameTypes = null,
            [FromQuery] TermNameType[] excludeNameTypes = null
        )
        {
            if (size <= 0)
                size = 100;

            if (from < 0)
                from = 0;

            // If Resource types to retrieve aren't specified, get all.
            if (includeResourceTypes == null || includeResourceTypes.Length == 0)
                includeResourceTypes = ALL_DRUG_RESOURCE_TYPES;

            // If term name types to retrieve aren't specified, get all.
            if (includeNameTypes == null || includeNameTypes.Length == 0)
                includeNameTypes = ALL_TERM_NAME_TYPES;

            // If term name types to exclude aren't specified, exclude none.
            if (excludeNameTypes == null)
                excludeNameTypes = new TermNameType[0];

            DrugTermResults res = await _termsQueryService.GetAll(size, from,
                includeResourceTypes, includeNameTypes, excludeNameTypes);

            return res;
        }

        /// <summary>
        /// Get the drug definition by its ID
        /// </summary>
        /// <returns>DrugTerm object</returns>
        [HttpGet("{id:long}")]
        public async Task<DrugTerm> GetById(long id)
        {
            if(id <= 0)
            {
                throw new APIErrorException(400, $"Not a valid ID '{id}'.");
            }

            DrugTerm res = await _termsQueryService.GetById(id);
            return res;
        }

        /// <summary>
        /// Get the dictionary entry based on Pretty URL Name.
        /// </summary>
        /// <param name="prettyUrlName">The PrettyUrlName of the term to fetch. Required.</param>
        /// <returns>The requested dictionary entry.</returns>
        [HttpGet("{prettyUrlName}")]
        public async Task<DrugTerm> GetByName(string prettyUrlName)
        {
            if (String.IsNullOrWhiteSpace(prettyUrlName))
                throw new APIErrorException(400, "You must specify the prettyUrlName parameter.");

            return await _termsQueryService.GetByName(prettyUrlName);
        }

        /// <summary>
        /// Retrieves a portion of the overall set of drug definitions that match a specific search criteria. This will return
        /// only DrugTerms, but will match against the Drug Term's name OR any of its Aliases.
        /// </summary>
        /// <param name="query">The search text. Required.</param>
        /// <param name="matchType">Should the search match items beginning with the search text, or containing it? Default: Begin.</param>
        /// <param name="size">The number of records to retrieve. Default: 100.</param>
        /// <param name="from">The offset into the overall set to use for the first record.</param>
        /// <returns>A DrugTermResults object containing the desired records.</returns>
        [HttpGet("search")]
        public async Task<DrugTermResults> Search(
            [FromQuery] string query,
            [FromQuery] MatchType matchType = MatchType.Begins,
            [FromQuery] int size = 100,
            [FromQuery] int from = 0
        )
        {
            if(String.IsNullOrWhiteSpace(query))
                throw new APIErrorException(400, "You must specify a search string.");

            if (!Enum.IsDefined(typeof(MatchType), matchType))
                throw new APIErrorException(400, "The `matchType` parameter must be either 'Begins' or 'Contains'.");

            if (size <= 0)
                size = 100;

            if (from < 0)
                from = 0;

            // query uses a catch-all route, make sure it's been decoded.
            query = WebUtility.UrlDecode(query);

            DrugTermResults res = await _termsQueryService.Search(query, matchType, size, from);
            return res;
        }

    }
}