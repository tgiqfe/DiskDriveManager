using DiskDriveManager.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiskDriveManager.DiskDrive
{
    internal class DriveItem
    {
        public string Label { get; set; }
        public string Path { get; set; }
        public string DriveLetter { get; set; }
        public UInt16 FileSYstemType { get; set; }
        public ulong Size { get; set; }
        public ulong SizeFree { get; set; }
        public string SizeText { get { return TextFunctions.FormatFileSize(Size); }  }
        public string SizeFreeText { get { return TextFunctions.FormatFileSize(SizeFree); } }
        public uint DriveType { get; set; }
    }
}
