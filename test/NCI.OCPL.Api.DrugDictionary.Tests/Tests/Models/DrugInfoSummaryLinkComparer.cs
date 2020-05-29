using System.Collections.Generic;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// An IEqualityComparer for TermAlias objects.
    /// </summary>
    public class DrugInfoSummaryLinkComparer : IEqualityComparer<DrugInfoSummaryLink>
    {
        public bool Equals(DrugInfoSummaryLink x, DrugInfoSummaryLink y)
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
                x.URI == y.URI &&
                x.Text == y.Text
            ;

            return isEqual;
        }

        public int GetHashCode(DrugInfoSummaryLink obj)
        {
            int hash = 0;

            hash ^=
                obj.URI.GetHashCode()
                ^ obj.Text.GetHashCode()
            ;

            return hash;
        }
    }
}