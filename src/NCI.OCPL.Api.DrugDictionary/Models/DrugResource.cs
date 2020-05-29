using Nest;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// This class implements the properties defined in IDrugResource, however it
    /// expressly does *NOT* "implement" IDrugResource. The intent is to enforce the
    /// use of classes implementing IDrugResource for serializiation, while providing
    /// a single class for maintaining the NEST attribute mapping data.
    /// </summary>
    public class DrugResource
    {
        /// <summary>
        /// The CDR ID of the full term for a DrugTerm, or the full term this alias represents.
        /// </summary>
        [Number(NumberType.Long, Name = "term_id")]
        public long TermId { get; set; }

        /// <summary>
        /// The name of this resource as it should be displayed on the listing pages
        /// </summary>
        [Keyword(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The first letter of the term as would be used by the expand endpoint
        /// </summary>
        [Keyword(Name = "first_letter")]
        public char FirstLetter { get; set; }

        /// <summary>
        /// The type of this resource.
        /// </summary>
        [Keyword(Name = "type")]
        public DrugResourceType Type { get; set; }

        /// <summary>
        /// The type of name for this resource.
        /// </summary>
        [Keyword(Name = "term_name_type")]
        public TermNameType TermNameType { get; set; }

        /// <summary>
        /// The url fragment used in the full defintion URL of /def/&lt;PrettyUrlName&gt;.
        /// </summary>
        [Keyword(Name = "pretty_url_name")]
        public string PrettyUrlName { get; set; }
    }
}