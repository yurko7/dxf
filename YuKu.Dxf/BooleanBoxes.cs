using System;

namespace YuKu.Dxf
{
    internal static class BooleanBoxes
    {
        internal static readonly Object TrueBox = true;
        internal static readonly Object FalseBox = false;

        internal static Object Box(Boolean value)
        {
            return value ? TrueBox : FalseBox;
        }
    }
}
