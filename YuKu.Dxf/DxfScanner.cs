using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace YuKu.Dxf
{
    /// <summary>
    /// Reads DXF groups.
    /// </summary>
    /// <seealso cref="DxfGroup"/>
    public sealed partial class DxfScanner : IEnumerable<DxfGroup>
    {
        public DxfScanner(String fileName)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public String FileName { get; }

        public IEnumerator<DxfGroup> GetEnumerator()
        {
            var fileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return new AsciiDxfEnumerator(fileStream);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static Type GetValueType(Int16 groupCode)
        {
            if (groupCode.IsInRange(0, 9)
                || groupCode == 100
                || groupCode == 102
                || groupCode == 105
                || groupCode.IsInRange(300, 309)
                || groupCode.IsInRange(320, 329)
                || groupCode.IsInRange(330, 369)
                || groupCode.IsInRange(390, 399)
                || groupCode.IsInRange(410, 419)
                || groupCode.IsInRange(430, 439)
                || groupCode.IsInRange(470, 479)
                || groupCode.IsInRange(480, 481)
                || groupCode == 999
                || groupCode.IsInRange(1000, 1003)
                || groupCode.IsInRange(1005, 1009))
            {
                return typeof(String);
            }

            if (groupCode.IsInRange(10, 39)
                || groupCode.IsInRange(40, 59)
                || groupCode.IsInRange(110, 119)
                || groupCode.IsInRange(120, 129)
                || groupCode.IsInRange(130, 139)
                || groupCode.IsInRange(140, 149)
                || groupCode.IsInRange(210, 239)
                || groupCode.IsInRange(460, 469)
                || groupCode.IsInRange(1010, 1059))
            {
                return typeof(Double);
            }

            if (groupCode.IsInRange(60, 79)
                || groupCode.IsInRange(170, 179)
                || groupCode.IsInRange(270, 279)
                || groupCode.IsInRange(280, 289)
                || groupCode.IsInRange(370, 379)
                || groupCode.IsInRange(380, 389)
                || groupCode.IsInRange(400, 409)
                || groupCode.IsInRange(1060, 1070))
            {
                return typeof(Int16);
            }

            if (groupCode.IsInRange(90, 99)
                || groupCode.IsInRange(420, 429)
                || groupCode.IsInRange(440, 449)
                || groupCode.IsInRange(450, 459)
                || groupCode == 1071)
            {
                return typeof(Int32);
            }

            if (groupCode.IsInRange(160, 169))
            {
                return typeof(Int64);
            }

            if (groupCode.IsInRange(290, 299))
            {
                return typeof(Boolean);
            }

            if (groupCode.IsInRange(310, 319)
                || groupCode == 1004)
            {
                return typeof(Byte[]);
            }

            throw new InvalidDataException($"Unknown group code: {groupCode}");
        }
    }
}
