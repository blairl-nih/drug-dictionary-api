namespace NCI.OCPL.Api.DrugDictionary.Models
{
    /// <summary>
    /// Configuration options for the Drug Dictionary Term API.
    /// </summary>
    public class DrugDictionaryAPIOptions
    {
        /// <summary>
        /// Gets or sets the alias name for the Elasticsearch Collection we will use.
        /// </summary>
        /// <value>The name of the alias.</value>
        public string AliasName { get; set; }

        /// <summary>
        /// Gets or sets configuration options specific to Autosuggest.
        /// </summary>
        /// <value>An object containing the configuration items specific to autosuggest.</value>
        public AutosuggestOptions Autosuggest { get; set; }
    }
}