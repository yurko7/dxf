using System;

namespace YuKu.Dxf
{
    internal static class Int64Boxes
    {
        internal static readonly Object MinusOneBox = -1L;
        internal static readonly Object ZeroBox = 0L;
        internal static readonly Object OneBox = 1L;

        internal static Object Box(Int64 value)
        {
            switch (value)
            {
                case -1L:
                    return MinusOneBox;
                case 0L:
                    return ZeroBox;
                case 1L:
                    return OneBox;
                default:
                    return value;
            }
        }
    }
}
