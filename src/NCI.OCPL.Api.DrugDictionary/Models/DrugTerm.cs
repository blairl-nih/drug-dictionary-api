using Nest;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Data structure representing a single drug dictionary entry.
    /// </summary>
    public class DrugTerm : DrugResource, IDrugResource
    {
        /// <summary>
        /// Array of aliases for the drug.
        /// </summary>
        [Nested(Name = "aliases")]
        public TermAlias[] Aliases { get; set; }

        /// <summary>
        /// The drug's definition.
        /// </summary>
        [Nested(Name = "definition")]
        public Definition Definition { get; set; }

        /// <summary>
        /// Link to a matching Drug Information Summary.
        /// </summary>
        [Nested(Name = "drug_info_summary_link")]
        public DrugInfoSummaryLink DrugInfoSummaryLink { get; set; }

        /// <summary>
        /// The NCI Concept ID.
        /// </summary>
        [Text(Name = "nci_concept_id")]
        public string NCIConceptId { get; set; }

        /// <summary>
        /// The NCI Concept Name.
        /// </summary>
        [Text(Name = "nci_concept_name")]
        public string NCIConceptName { get; set; }
    }
}