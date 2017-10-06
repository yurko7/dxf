using System;
using System.Collections.Generic;

namespace YuKu.Dxf
{
    internal sealed class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
    {
        public static ArrayEqualityComparer<T> Default => _default ?? (_default = new ArrayEqualityComparer<T>());

        public Boolean Equals(T[] x, T[] y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Length != y.Length)
            {
                return false;
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (Int32 i = 0; i < x.Length; ++i)
            {
                if (!comparer.Equals(x[i], y[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public Int32 GetHashCode(T[] obj)
        {
            return obj.GetHashCode();
        }

        private static ArrayEqualityComparer<T> _default;
    }
}
