using Nest;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Data structure representing one alias for a drug term.
    /// </summary>
    public class TermAlias
    {
        /// <summary>
        /// The type of alias.
        /// </summary>
        [Keyword(Name = "type")]
        public TermNameType Type { get; set; }

        /// <summary>
        /// The actual alias name.
        /// </summary>
        [Text(Name = "name")]
        public string Name { get; set; }
    }
}