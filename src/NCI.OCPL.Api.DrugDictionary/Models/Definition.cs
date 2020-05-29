using Nest;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Contains the definition of a drug dictionary entry.
    /// </summary>
    public class Definition
    {
        /// <summary>
        /// The definition, rendered as HTML.
        /// </summary>
        [Text(Name = "html")]
        public string Html { get; set; }

        /// <summary>
        /// The definition, rendered as plain text.
        /// </summary>
        [Text(Name = "text")]
        public string Text { get; set; }
    }
}