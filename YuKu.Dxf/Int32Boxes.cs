using System;

namespace YuKu.Dxf
{
    internal static class Int32Boxes
    {
        internal static readonly Object MinusOneBox = -1;
        internal static readonly Object ZeroBox = 0;
        internal static readonly Object OneBox = 1;

        internal static Object Box(Int32 value)
        {
            switch (value)
            {
                case -1:
                    return MinusOneBox;
                case 0:
                    return ZeroBox;
                case 1:
                    return OneBox;
                default:
                    return value;
            }
        }
    }
}
