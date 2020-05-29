using System.Collections.Generic;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// An IEqualityComparer for TermAlias objects.
    /// </summary>
    public class TermAliasComparer : IEqualityComparer<TermAlias>
    {
        public bool Equals(TermAlias x, TermAlias y)
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
                x.Type == y.Type &&
                x.Name == y.Name
            ;

            return isEqual;
        }

        public int GetHashCode(TermAlias obj)
        {
            int hash = 0;

            hash ^=
                obj.Type.GetHashCode()
                ^ obj.Name.GetHashCode()
            ;

            return hash;
        }
    }
}