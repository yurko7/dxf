using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YuKu.Dxf
{
    partial class DxfScanner
    {
        private sealed class BinaryDxfEnumerator : IEnumerator<DxfGroup>
        {
            public BinaryDxfEnumerator(Stream stream)
            {
                if (stream == null)
                {
                    throw new ArgumentNullException(nameof(stream));
                }
                _binaryReader = new BinaryReader(stream, Encoding.ASCII);
            }

            public DxfGroup Current { get; private set; }

            Object IEnumerator.Current => Current;

            public Boolean IsDisposed { get; private set; }

            public Boolean MoveNext()
            {
                try
                {
                    Int16 groupCode = ReadGroupCode();
                    Type valueType = GetValueType(groupCode);
                    Object value;
                    if (valueType == typeof(String))
                    {
                        value = ReadString();
                    }
                    else if (valueType == typeof(Double))
                    {
                        value = _binaryReader.ReadDouble();
                    }
                    else if (valueType == typeof(Int16))
                    {
                        value = Int16Boxes.Box(_binaryReader.ReadInt16());
                    }
                    else if (valueType == typeof(Int32))
                    {
                        value = Int32Boxes.Box(_binaryReader.ReadInt32());
                    }
                    else if (valueType == typeof(Int64))
                    {
                        value = Int64Boxes.Box(_binaryReader.ReadInt64());
                    }
                    else if (valueType == typeof(Boolean))
                    {
                        value = BooleanBoxes.Box(_binaryReader.ReadBoolean());
                    }
                    else if (valueType == typeof(Byte[]))
                    {
                        value = ReadBinaryChunk();
                    }
                    else
                    {
                        throw new InvalidDataException($"Unknown value type. Value group code: {groupCode}");
                    }

                    Current = new DxfGroup(groupCode, value);
                    return true;
                }
                catch (EndOfStreamException)
                {
                    Current = default;
                    return false;
                }
            }

            public void Reset()
            {
                _binaryReader.BaseStream.Seek(BinaryDxfSentinel.Length, SeekOrigin.Begin);
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private Int16 ReadGroupCode()
            {
                Byte groupCodeLo = _binaryReader.ReadByte();
                Boolean extended = groupCodeLo == 0xFF;
                Int16 groupCode;
                if (extended)
                {
                    groupCode = _binaryReader.ReadInt16();
                }
                else
                {
                    Byte groupCodeHi = _binaryReader.ReadByte();
                    groupCode = (Int16) ((groupCodeHi << 8) | groupCodeLo);
                }
                return groupCode;
            }

            private String ReadString()
            {
                var bytes = new List<Byte>(128);
                Byte ch = _binaryReader.ReadByte();
                while (ch != 0)
                {
                    bytes.Add(ch);
                    ch = _binaryReader.ReadByte();
                }
                return Encoding.ASCII.GetString(bytes.ToArray());
            }

            private Byte[] ReadBinaryChunk()
            {
                Byte chunkLength = _binaryReader.ReadByte();
                Byte[] chunkData = _binaryReader.ReadBytes(chunkLength);
                return chunkData;
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
                    _binaryReader.Dispose();
                }
            }

            ~BinaryDxfEnumerator() => Dispose(false);

            private readonly BinaryReader _binaryReader;
        }
    }
}
