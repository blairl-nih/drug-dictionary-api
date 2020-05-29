using System;
using System.Linq;
using System.Collections.Generic;

using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// A IEqualityComparer for DrugTerm
    /// </summary>
    public class MetaLinkComparer : IEqualityComparer<Metalink>
    {
        public bool Equals(Metalink x, Metalink y)
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
                x.Self == y.Self
            ;

            return isEqual;
            }

            public int GetHashCode(Metalink obj)
            {
            int hash = 0;
            hash ^=
                obj.Self.GetHashCode()
            ;

            return hash;
        }

    }
}