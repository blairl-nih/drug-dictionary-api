using Newtonsoft.Json;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Interface defining the fields common to all dictionary entries.
    /// </summary>
    [JsonConverter(typeof(DrugResourceConverter))]
    public interface IDrugResource
    {
        /// <summary>
        /// The CDR ID of the full term for a DrugTerm, or the full term this alias represents.
        /// </summary>
        long TermId {get;set;}

        /// <summary>
        /// The name of this resource as it should be displayed on the listing pages
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The first letter of the term as would be used by the expand endpoint
        /// </summary>
        char FirstLetter { get; set; }

        /// <summary>
        /// The type of this resource.
        /// </summary>
        DrugResourceType Type { get; set; }


        /// <summary>
        /// The type of name for this resource.
        /// </summary>
        TermNameType TermNameType { get; set; }

        /// <summary>
        /// The url fragment used in the full defintion URL of /def/&lt;PrettyUrlName&gt;.
        /// </summary>
        string PrettyUrlName { get; set; }

    }
}