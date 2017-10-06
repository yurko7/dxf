using System;

namespace YuKu.Dxf
{
    internal static class Extensions
    {
        public static Boolean IsInRange(this Int16 number, Int16 from, Int16 to)
        {
            return from <= number && number <= to;
        }
    }
}
