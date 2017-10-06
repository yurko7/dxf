using System;

namespace YuKu.Dxf
{
    internal static class Int16Boxes
    {
        internal static readonly Object MinusOneBox = (Int16) (-1);
        internal static readonly Object ZeroBox = (Int16) 0;
        internal static readonly Object OneBox = (Int16) 1;

        internal static Object Box(Int16 value)
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
