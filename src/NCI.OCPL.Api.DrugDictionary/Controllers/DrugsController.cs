using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

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
        /// <param name="requestedFields">The fields to retrieve.  If not specified, defaults to all fields except media and related resources.</param>
        /// <returns>A DrugTermResults object containing the desired records.</returns>
        [HttpGet("expand/{character}")]
        public async Task<DrugTermResults> Expand(char character,
            [FromQuery] int size = 100, [FromQuery] int from = 0,
            [FromQuery] DrugResourceType[] includeResourceTypes = null,
            [FromQuery] TermNameType[] includeNameTypes = null,
            [FromQuery] TermNameType[] excludeNameTypes = null,
            [FromQuery] string[] requestedFields = null
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

            if (requestedFields == null || requestedFields.Length == 0 || requestedFields.Where(f => !String.IsNullOrWhiteSpace(f)).Count() == 0)
                requestedFields = DEFAULT_FIELD_NAMES;

            DrugTermResults res = await _termsQueryService.Expand(character, size, from,
                includeResourceTypes, includeNameTypes, excludeNameTypes, requestedFields);

            return res;
        }

    }
}