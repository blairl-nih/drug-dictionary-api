namespace NCI.OCPL.Api.DrugDictionary.Models
{
    /// <summary>
    /// Configuration options specifically for autosuggest.
    /// </summary>
    public class AutosuggestOptions
    {
        /// <summary>
        /// Maximum number of characters to include in a suggested search term.
        /// </summary>
        public int MaxSuggestionLength { get; set; }
    }
}
