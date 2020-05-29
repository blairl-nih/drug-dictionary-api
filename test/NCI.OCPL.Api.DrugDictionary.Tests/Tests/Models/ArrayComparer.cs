using System.Collections.Generic;
using System.Linq;

namespace NCI.OCPL.Api.DrugDictionary.Tests
{

    /// <summary>
    /// Generic comparer for arrays of any arbitrary type T.
    /// </summary>
    /// <typeparam name="T">The type of item in the arrays being compared.</typeparam>
    /// <typeparam name="V">A type which implements IEqualityComparer<T>.</typeparam>
    public class ArrayComparer<T, V> : IEqualityComparer<T[]> where V : IEqualityComparer<T>, new()
    {

        /// <summary>
        /// Compares two arrays of type T where order is important.
        /// </summary>
        /// <param name="x">First array to compare</param>
        /// <param name="y">Second array to compare</param>
        /// <returns>True if the arrays contain an identical set of objects in the same order; false otherwise.</returns>
        public bool Equals(T[] x, T[] y)
        {
            IEqualityComparer<T> comparer = new V();

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
            else if (x.Count() != y.Count())
            {
                return false;
            }

            for (int i = 0; i < x.Length; i++)
            {
                if (!comparer.Equals(x[i], y[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two arrays of type T without regard to order.
        /// </summary>
        /// <param name="x">First array to compare</param>
        /// <param name="y">Second array to compare</param>
        /// <returns>True if the arrays contain an identical set of objects in the same order; false otherwise.</returns>
        public bool Equivalent(T[] x, T[] y)
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

            if (x.Count() != y.Count())
            {
                return false;
            }

            //Generate a set of those values that are not in both lists.
            //if this is not 0, then there is an error.
            var diffxy = x.Except(y, new V());

            return diffxy.Count() == 0;
        }


        /// <summary>
        /// Returns a hash code for the specified array.
        /// </summary>
        /// <param name="obj">The array for which a hash code is to be returned.</param>
        /// <returns>A hash code.</returns>
        public int GetHashCode(T[] obj)
        {
            IEqualityComparer<T> comparer = new V();

            int hash = 0;
            foreach (T item in obj)
            {
                hash ^= comparer.GetHashCode(item);
            }

            return hash;
        }

    }
}