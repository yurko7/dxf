using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace YuKu.Dxf
{
    partial class DxfScanner
    {
        private sealed class AsciiDxfEnumerator : IEnumerator<DxfGroup>
        {
            public AsciiDxfEnumerator(Stream stream)
            {
                if (stream == null)
                {
                    throw new ArgumentNullException(nameof(stream));
                }
                _streamReader = new StreamReader(stream);
            }

            public DxfGroup Current { get; private set; }

            Object IEnumerator.Current => Current;

            public Boolean IsDisposed { get; private set; }

            public Boolean MoveNext()
            {
                try
                {
                    String strGroupCode = _streamReader.ReadLine();
                    if (strGroupCode == null)
                    {
                        Current = default;
                        return false;
                    }
                    Int16 groupCode = Int16.Parse(strGroupCode);
                    Type valueType = GetValueType(groupCode);

                    String strData = _streamReader.ReadLine();
                    Object value;
                    if (valueType == typeof(String))
                    {
                        value = strData;
                    }
                    else if (valueType == typeof(Boolean))
                    {
                        Boolean boolValue = Convert.ToBoolean(Convert.ToByte(strData));
                        value = BooleanBoxes.Box(boolValue);
                    }
                    else if (valueType == typeof(Int16))
                    {
                        Int16 int16Value = Convert.ToInt16(strData);
                        value = Int16Boxes.Box(int16Value);
                    }
                    else if (valueType == typeof(Int32))
                    {
                        Int32 int32Value = Convert.ToInt32(strData);
                        value = Int32Boxes.Box(int32Value);
                    }
                    else if (valueType == typeof(Int64))
                    {
                        Int64 int64Value = Convert.ToInt64(strData);
                        value = Int64Boxes.Box(int64Value);
                    }
                    else if (valueType == typeof(Byte[]))
                    {
                        value = HexStringToByteArray(strData);
                    }
                    else
                    {
                        value = Convert.ChangeType(strData, valueType, CultureInfo.InvariantCulture);
                    }
                    Current = new DxfGroup(groupCode, value);
                    return true;
                }
                catch (FormatException e)
                {
                    throw new InvalidDataException("Error reading DXF value.", e);
                }
            }

            public void Reset()
            {
                _streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private static Byte[] HexStringToByteArray(String hexString)
            {
                var bytes = new Byte[hexString.Length / 2];
                for (Int32 i = 0; i < bytes.Length; ++i)
                {
                    String strByte = hexString.Substring(i * 2, 2);
                    bytes[i] = Convert.ToByte(strByte, 16);
                }
                return bytes;
            }

            private void Dispose(Boolean disposing)
            {
                if (IsDisposed)
                {
                    return;
                }
                IsDisposed = true;
                if (disposing)
                {
                    _streamReader.Dispose();
                }
            }

            ~AsciiDxfEnumerator() => Dispose(false);

            private readonly StreamReader _streamReader;
        }
    }
}
