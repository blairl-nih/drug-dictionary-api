using Nest;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Represents a record containing an alias for a <see cref="T:NCI.OCPL.Api.DrugDictionary.DrugTerm"/>.
    /// </summary>
    public class DrugAlias : DrugResource, IDrugResource
    {
        /// <summary>
        /// The drug's preferred name.
        /// </summary>
        [Keyword(Name = "preferred_name")]
        public string PreferredName { get; set; }
    }
}