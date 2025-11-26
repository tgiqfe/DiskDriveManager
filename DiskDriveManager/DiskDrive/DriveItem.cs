using DiskDriveManager.Functions;
using DiskDriveManager.Functions.EnumParser;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using System.Text.Json.Serialization;

namespace DiskDriveManager.DiskDrive
{
    /// <summary>
    /// Drive information class
    /// [PowerShell example]
    /// $wmi_dsv = Get-CimInstance -ClassName "MSFT_Volume" -Namespace "\\.\root\Microsoft\Windows\Storage"
    /// $air = Get-CimInstance -ClassName "MSFT_PartitionToVolume" -Namespace "\\.\root\Microsoft\Windows\Storage"
    /// </summary>
    internal class DriveItem
    {
        #region Public parameter

        public string Label { get; set; }
        public string Path { get; set; }
        public string DriveLetter { get; set; }
        public FileSystemType FileSystemType { get; set; }
        public ulong Size { get; set; }
        public ulong SizeFree { get; set; }
        public string SizeText { get { return TextFunctions.FormatFileSize(Size); } }
        public string SizeFreeText { get { return TextFunctions.FormatFileSize(SizeFree); } }
        public DriveType DriveType { get; set; }

        // Setting with DiskDriveHelper
        public uint DiskNumber { get; set; }
        public uint PartitionNumber { get; set; }

        [JsonIgnore]
        public string ObjectId { get; private set; }

        #endregion

        public DriveItem(ManagementObject wmi_volume)
        {
            this.Label = wmi_volume["FileSystemLabel"] as string;
            this.Path = wmi_volume["Path"] as string;
            this.DriveLetter = wmi_volume["DriveLetter"] as string;
            this.FileSystemType = FileSystemTypeParser.RawToParam((ushort)wmi_volume["FileSystemType"]);
            this.Size = (ulong)(wmi_volume["Size"] ?? 0UL);
            this.SizeFree = (ulong)(wmi_volume["SizeRemaining"] ?? 0UL);
            this.DriveType = DriveTypeParser.RawToParam((uint)wmi_volume["DriveType"]);
            this.ObjectId = wmi_volume["ObjectId"] as string;
        }

        public static IEnumerable<DriveItem> Load()
        {
            var wmi_volumes = new ManagementClass(@"\\.\root\Microsoft\Windows\Storage", "MSFT_Volume", new ObjectGetOptions()).GetInstances().OfType<ManagementObject>();

            return wmi_volumes.Select(x => new DriveItem(x));
        }
    }
}
