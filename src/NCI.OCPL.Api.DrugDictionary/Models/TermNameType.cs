using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nest;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Identifiers for the various types of names a term might have.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TermNameType
    {
        /// <summary>
        /// EVS code name.
        /// </summary>
        CodeName,

        /// <summary>
        /// An obsolete name.
        /// </summary>
        ObsoleteName,

        /// <summary>
        /// An abbreviation.
        /// </summary>
        Abbreviation,

        /// <summary>
        /// IND Code.
        /// </summary>
        INDCode,

        /// <summary>
        /// NSC Number.
        /// </summary>
        NSCNumber,

        /// <summary>
        /// Foreign brand name.
        /// </summary>
        ForeignBrandName,

        /// <summary>
        /// Synonym.
        /// </summary>
        Synonym,

        /// <summary>
        /// CAS Registry Name.
        /// </summary>
        CASRegistryName,

        /// <summary>
        /// Subtype.
        /// </summary>
        Subtype,

        /// <summary>
        /// Spanish name.
        /// </summary>
        Spanish,

        /// <summary>
        /// Lexical variant.
        /// </summary>
        LexicalVariant,

        /// <summary>
        /// Chemical structure name.
        /// </summary>
        ChemicalStructureName,

        /// <summary>
        /// Common usage.
        /// </summary>
        CommonUsage,

        /// <summary>
        /// Acronym.
        /// </summary>
        Acronym,

        /// <summary>
        /// US Brand name.
        /// </summary>
        USBrandName,

        /// <summary>
        /// Broader.
        /// </summary>
        Broader,

        /// <summary>
        /// Related string.
        /// </summary>
        RelatedString,

        /// <summary>
        /// Preferred name.
        /// </summary>
        PreferredName
    }
}