using System;

namespace YuKu.Dxf
{
    public struct DxfGroup : IEquatable<DxfGroup>
    {
        public static Boolean operator ==(DxfGroup left, DxfGroup right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(DxfGroup left, DxfGroup right)
        {
            return !left.Equals(right);
        }

        public DxfGroup(Int16 code, Object value)
        {
            Code = code;
            Value = value;
        }

        public Int16 Code { get; }

        public Object Value { get; }

        public override Boolean Equals(Object obj)
        {
            return obj is DxfGroup group && Equals(group);
        }

        public Boolean Equals(DxfGroup other)
        {
            Boolean equals = Code == other.Code;
            if (equals)
            {
                equals = Value is Byte[] byteArray
                    ? ArrayEqualityComparer<Byte>.Default.Equals(byteArray, other.Value as Byte[])
                    : Equals(Value, other.Value);
            }
            return equals;
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                Int32 valueHashCode = Value != null ? Value.GetHashCode() : 128;
                return (Code.GetHashCode() * 397) ^ valueHashCode;
            }
        }
    }
}
