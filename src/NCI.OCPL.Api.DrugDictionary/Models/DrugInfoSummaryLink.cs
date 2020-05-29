using System;

using Nest;

namespace NCI.OCPL.Api.DrugDictionary
{
    /// <summary>
    /// Contains information for creating a link to a drug information summary.
    /// </summary>
    public class DrugInfoSummaryLink
    {
        /// <summary>
        /// The URL of the drug information summary.
        /// </summary>
        [Text(Name = "url")]
        public Uri URI { get; set; }

        /// <summary>
        /// Link text.
        /// </summary>
        [Text(Name = "text")]
        public string Text { get; set; }
    }
}