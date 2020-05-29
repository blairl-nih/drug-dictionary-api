using System;
using System.Linq;
using System.Collections.Generic;


namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// A IEqualityComparer for DrugAlias
    /// </summary>
    public class DrugAliasComparer : IEqualityComparer<DrugAlias>
    {
        public bool Equals(DrugAlias x, DrugAlias y)
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
                x.TermId == y.TermId
                && x.Name == y.Name
                && x.FirstLetter == y.FirstLetter
                && x.Type == y.Type
                && x.TermNameType == y.TermNameType
                && x.PrettyUrlName == y.PrettyUrlName
                && x.PreferredName == y.PreferredName
            ;

            return isEqual;
            }

            public int GetHashCode(DrugAlias obj)
            {
            int hash = 0;
            hash ^=
                obj.TermId.GetHashCode()
                ^ obj.Name.GetHashCode()
                ^ obj.FirstLetter.GetHashCode()
                ^ obj.Type.GetHashCode()
                ^ obj.TermNameType.GetHashCode()
                ^ (obj.PrettyUrlName != null ? obj.PrettyUrlName.GetHashCode() : 0)
                ^ obj.PreferredName.GetHashCode()
            ;

            return hash;
        }

    }
}