using Nest;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Describes a single suggestion from autosuggest
    /// </summary>
    public class Suggestion
    {
        /// <summary>
        /// The term's CDR ID.
        /// </summary>
        /// <value></value>
        [Number(Name = "term_id")]
        public long TermId { get; set; }

        /// <summary>
        /// Gets or sets the Name of the Drug Dictionary Term.
        /// </summary>
        [Keyword(Name = "name")]
        public string TermName { get; set; }
    }
}
