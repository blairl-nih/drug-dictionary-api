using System;
using System.Linq;
using System.Collections.Generic;

using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// A IEqualityComparer for DrugTerm
    /// </summary>
    public class ResultsMetadataComparer : IEqualityComparer<ResultsMetadata>
    {
        public bool Equals(ResultsMetadata x, ResultsMetadata y)
        {
            // If the items are both null, or if one or the other is null, return
            // the correct response right away.
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            bool isEqual =
                x.TotalResults == y.TotalResults
                && x.From == y.From
            ;

            return isEqual;
            }

            public int GetHashCode(ResultsMetadata obj)
            {
            int hash = 0;
            hash ^=
                obj.TotalResults.GetHashCode()
                ^ obj.From.GetHashCode()
            ;

            return hash;
        }

    }
}