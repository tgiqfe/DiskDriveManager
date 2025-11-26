using System;
using System.Collections.Generic;
using System.Text;

namespace DiskDriveManager.DiskDrive
{
    internal enum FileSystemType
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
