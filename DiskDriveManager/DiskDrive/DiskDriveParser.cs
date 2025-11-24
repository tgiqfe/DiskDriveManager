
using DiskDriveManager.Functions;

namespace DiskDriveManager.DiskDrive
{
    internal class DiskDriveParser
    {
        #region PartitionStyle

        private static Dictionary<string[], PartitionStyle> _mapPartitionStyle = null;
        private static void InitializePartitionStyle()
        {
            _mapPartitionStyle = new()
            {
                { new string[] { "RAW", "Unknown", "0" }, PartitionStyle.Unknown },
                { new string[] { "MBR", "Master Boot Record", "MasterBootRecord", "1" }, PartitionStyle.MBR },
                { new string[] { "GPT", "GUID Partition Table", "GuidPartitionTable", "2" }, PartitionStyle.GPT },
            };
        }
        public static PartitionStyle StringToPartitionStyl(string text)
        {
            if (_mapPartitionStyle == null) InitializePartitionStyle();
            return TextFunctions.StringToFlags<PartitionStyle>(text, _mapPartitionStyle);
        }
        public static string PartitionStyleToString(PartitionStyle val)
        {
            if (_mapPartitionStyle == null) InitializePartitionStyle();
            return TextFunctions.FlagsToString<PartitionStyle>(val, _mapPartitionStyle);
        }
        public static string GetPartitionStyleString(string text)
        {
            if (_mapPartitionStyle == null) InitializePartitionStyle();
            return TextFunctions.GetCorrect<PartitionStyle>(text, _mapPartitionStyle);
        }

        #endregion
        #region PhysicalDiskType

        private static Dictionary<string[], PhysicalDiskType> _mapPhysicalDiskType = null;
        private static void InitializePhysicalDiskType()
        {
            _mapPhysicalDiskType = new()
            {
                { new string[] { "Unspecified", "0" }, PhysicalDiskType.Unspecified },
                { new string[] { "HDD", "3", "Hard Disk Drive", "HardDiskDrive" }, PhysicalDiskType.HDD },
                { new string[] { "SSD", "4", "Solid State Drive", "SolidStateDrive" }, PhysicalDiskType.SSD },
                { new string[] { "SCM", "5", "Storage Class Memory", "StorageClassMemory" }, PhysicalDiskType.SCM },
                { new string[] { "Unknown", "-1" }, PhysicalDiskType.Unknown },
            };
        }
        public static PhysicalDiskType StringToPhysicalDiskType(string text)
        {
            if (_mapPhysicalDiskType == null) InitializePhysicalDiskType();
            return TextFunctions.StringToFlags<PhysicalDiskType>(text, _mapPhysicalDiskType);
        }
        public static string PhysicalDiskTypeToString(PhysicalDiskType val)
        {
            if (_mapPhysicalDiskType == null) InitializePhysicalDiskType();
            return TextFunctions.FlagsToString<PhysicalDiskType>(val, _mapPhysicalDiskType);
        }
        public static string GetPhysicalDiskTypeString(string text)
        {
            if (_mapPhysicalDiskType == null) InitializePhysicalDiskType();
            return TextFunctions.GetCorrect<PhysicalDiskType>(text, _mapPhysicalDiskType);
        }

        #endregion
    }
}
