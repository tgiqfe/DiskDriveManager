using System;
using System.Collections.Generic;
using System.Text;

namespace DiskDriveManager.Functions.EnumParser
{
    internal class PhysicalDiskTypeParser : ParserBase<PhysicalDiskType>
    {
        public PhysicalDiskTypeParser()
        {
            Initialize();
        }
        protected override void Initialize()
        {
            map = new()
            {
                { new string[] { "Unspecified", "0" }, PhysicalDiskType.Unspecified },
                { new string[] { "HDD", "3", "Hard Disk Drive", "HardDiskDrive" }, PhysicalDiskType.HDD },
                { new string[] { "SSD", "4", "Solid State Drive", "SolidStateDrive" }, PhysicalDiskType.SSD },
                { new string[] { "SCM", "5", "Storage Class Memory", "StorageClassMemory" }, PhysicalDiskType.SCM },
                { new string[] { "Unknown", "-1" }, PhysicalDiskType.Unknown },
            };
        }

        #region Static methods.

        private static PhysicalDiskTypeParser _parser = null;

        public static ushort ParamsToRaw(PhysicalDiskType flags)
        {
            _parser ??= new PhysicalDiskTypeParser();
            return (ushort)_parser.FlagsToNumber(flags);
        }

        public static PhysicalDiskType RawToParam(ushort num)
        {
            _parser ??= new PhysicalDiskTypeParser();
            return _parser.NumberToFlags(Convert.ToInt32(num));
        }

        #endregion
    }

    internal enum PhysicalDiskType
    {
        Unspecified = 0,
        HDD = 3,
        SSD = 4,
        SCM = 5,
        Unknown = -1,
    }
}
