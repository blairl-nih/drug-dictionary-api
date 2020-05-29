using System;
using System.Linq;
using System.Collections.Generic;

using NCI.OCPL.Api.DrugDictionary;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{
    /// <summary>
    /// A IEqualityComparer for object implementing IDrugResource.
    /// </summary>
    public class IDrugResourceComparer : IEqualityComparer<IDrugResource>
    {
        public bool Equals(IDrugResource x, IDrugResource y)
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

            // Must be the same type of object.
            if(x.Type != y.Type)
            {
                return false;
            }

            // Since we now know they're the same type, we can use the type of X
            // to figure out which specific type's comparer to use.
            switch(x)
            {
                case DrugAlias da:
                    return new DrugAliasComparer().Equals((DrugAlias)x,(DrugAlias)y);

                case DrugTerm dt:
                    return new DrugTermComparer().Equals((DrugTerm)x, (DrugTerm)y);

                default:
                    throw new ArgumentException(
                        message: "x and y are not a recognized implmentation of IDrugResource.",
                        paramName: nameof(x)
                    );
            }
        }

        public int GetHashCode(IDrugResource obj)
        {
            switch(obj)
            {
                case DrugAlias da:
                    return new DrugAliasComparer().GetHashCode(da);

                case DrugTerm dt:
                    return new DrugTermComparer().GetHashCode(dt);

                default:
                    throw new ArgumentException(
                        message: "obj is not a recognized implmentation of IDrugResource.",
                        paramName: nameof(obj)
                    );
            }
        }

    }
}