using DiskDriveManager.Functions;
using DiskDriveManager.Functions.EnumParser;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace DiskDriveManager.DiskDrive
{
    /// <summary>
    /// Drive information class
    /// [PowerShell example]
    /// $wmi_dsv = Get-CimInstance -ClassName "MSFT_Volume" -Namespace "\\.\root\Microsoft\Windows\Storage"
    /// </summary>
    internal class DriveItem
    {
        public string Label { get; set; }
        public string Path { get; set; }
        public string DriveLetter { get; set; }
        public FileSystemType FileSystemType { get; set; }
        public ulong Size { get; set; }
        public ulong SizeFree { get; set; }
        public string SizeText { get { return TextFunctions.FormatFileSize(Size); } }
        public string SizeFreeText { get { return TextFunctions.FormatFileSize(SizeFree); } }
        public DriveType DriveType { get; set; }

        public DriveItem(ManagementObject wmi_storageVolume)
        {
            this.Label = wmi_storageVolume["FileSystemLabel"] as string;
            this.Path = wmi_storageVolume["Path"] as string;
            this.DriveLetter = wmi_storageVolume["DriveLetter"] as string;
            this.FileSystemType = FileSystemTypeParser.RawToParam((ushort)wmi_storageVolume["FileSystemType"]);
            this.Size = (ulong)(wmi_storageVolume["Size"] ?? 0UL);
            this.SizeFree = (ulong)(wmi_storageVolume["SizeRemaining"] ?? 0UL);
            this.DriveType = DriveTypeParser.RawToParam((uint)wmi_storageVolume["DriveType"]);
        }

        public static IEnumerable<DriveItem> Load()
        {
            var wmi_storageVolumes = new ManagementClass(@"\\.\root\Microsoft\Windows\Storage", "MSFT_Volume", new ObjectGetOptions()).GetInstances().OfType<ManagementObject>();

            return wmi_storageVolumes.Select(x => new DriveItem(x));
        }
    }
}
