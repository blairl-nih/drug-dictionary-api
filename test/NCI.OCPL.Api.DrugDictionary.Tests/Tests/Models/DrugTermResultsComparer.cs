using System.Collections.Generic;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// An IEqualityComparer for TermAlias objects.
    /// </summary>
    public class DrugTermResultsComparer : IEqualityComparer<DrugTermResults>
    {
        public bool Equals(DrugTermResults x, DrugTermResults y)
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
                new ResultsMetadataComparer().Equals(x.Meta, y.Meta)
                && new ArrayComparer<IDrugResource, IDrugResourceComparer>().Equals(x.Results, y.Results)
                && new MetaLinkComparer().Equals(x.Links, y.Links)
            ;

            return isEqual;
        }

        public int GetHashCode(DrugTermResults obj)
        {
            int hash = 0;

            hash ^=
                obj.Meta.GetHashCode()
                ^ obj.Results.GetHashCode()
                ^ obj.Links.GetHashCode()
            ;

            return hash;
        }
    }
}