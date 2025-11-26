using System;
using System.Collections.Generic;
using System.Text;

namespace DiskDriveManager.Functions.EnumParser
{
    internal class FileSystemTypeParser : ParserBase<FileSystemType>
    {
        public FileSystemTypeParser()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            map = new()
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

        #region Static methods.

        private static FileSystemTypeParser _parser = null;

        public static ushort ParamToRaw(FileSystemType flags)
        {
            _parser ??= new FileSystemTypeParser();
            return (ushort)_parser.FlagsToNumber(flags);
        }

        public static FileSystemType RawToParam(ushort val)
        {
            _parser ??= new FileSystemTypeParser();
            return _parser.NumberToFlags(val);
        }

        #endregion
    }

    public enum FileSystemType
    {
        Unknown = 0,
        UFS = 2,
        HFS = 3,
        FAT = 4,
        FAT16 = 5,
        FAT32 = 6,
        NTFS4 = 7,
        NTFS5 = 8,
        XFS = 9,
        AFS = 10,
        EXT2 = 11,
        EXT3 = 12,
        ReiserFS = 13,
        NTFS = 14,
        ReFS = 15,
        CSVFS_NTFS = 0x8000,
        CSVFS_REFS = 0x8001,
    }
}
