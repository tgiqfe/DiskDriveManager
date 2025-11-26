
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
        #region FileSystemType

        private static Dictionary<string[], FileSystemType> _mapFileSystemType = null;
        private static void InitializeFileSystemType()
        {
            _mapFileSystemType = new()
            {
                { new string[] { "Unknown", "0" }, FileSystemType.Unknown },
                { new string[] { "UFS", "2" }, FileSystemType.UFS },
                { new string[] { "HFS", "3" }, FileSystemType.HFS },
                { new string[] { "FAT", "4" }, FileSystemType.FAT },
                { new string[] { "FAT16", "5" }, FileSystemType.FAT16 },
                { new string[] { "FAT32", "6" }, FileSystemType.FAT32 },
                { new string[] { "NTFS4", "7" }, FileSystemType.NTFS4 },
                { new string[] { "NTFS5", "8" }, FileSystemType.NTFS5 },
                { new string[] { "XFS", "9" }, FileSystemType.XFS },
                { new string[] { "AFS", "10" }, FileSystemType.AFS },
                { new string[] { "EXT2", "11" }, FileSystemType.EXT2 },
                { new string[] { "EXT3", "12" }, FileSystemType.EXT3 },
                { new string[] { "ReiserFS", "13" }, FileSystemType.ReiserFS },
                { new string[] { "NTFS", "14" }, FileSystemType.NTFS },
                { new string[] { "ReFS", "15" }, FileSystemType.ReFS },
                { new string[] { "CSVFS_NTFS", "32768", "0x8000" }, FileSystemType.CSVFS_NTFS },
                { new string[] { "CSVFS_ReFS", "32769", "0x8001" }, FileSystemType.CSVFS_REFS },
            };
        }
        public static FileSystemType StringToFileSystemType(string text)
        {
            if (_mapFileSystemType == null) InitializeFileSystemType();
            return TextFunctions.StringToFlags<FileSystemType>(text, _mapFileSystemType);
        }
        public static string FileSystemTypeToString(FileSystemType val)
        {
            if (_mapFileSystemType == null) InitializeFileSystemType();
            return TextFunctions.FlagsToString<FileSystemType>(val, _mapFileSystemType);
        }
        public static int StringToInt(string text)
        {
            if (_mapFileSystemType == null) InitializeFileSystemType();
            return (int)TextFunctions.StringToFlags<FileSystemType>(text, _mapFileSystemType);
        }
        public static string IntToString(int number)
        {
            if (_mapFileSystemType == null) InitializeFileSystemType();
            return TextFunctions.FlagsToString<FileSystemType>(number, _mapFileSystemType);
        }
        public static string GetFileSystemTypeString(string text)
        {
            if (_mapFileSystemType == null) InitializeFileSystemType();
            return TextFunctions.GetCorrect<FileSystemType>(text, _mapFileSystemType);
        }

        #endregion
    }
}
