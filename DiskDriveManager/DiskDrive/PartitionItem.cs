using DiskDriveManager.Functions;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace DiskDriveManager.DiskDrive
{
    /// <summary>
    /// Partition information class
    /// [PowerShell example]
    /// $wmi_dsp = Get-CimInstance "Win32_DiskPartition"
    /// $wmi_php = Get-CimInstance -ClassName "MSFT_Partition" -Namespace "\\.\root\Microsoft\Windows\Storage"
    /// </summary>
    internal class PartitionItem
    {
        public uint DiskNumber { get; set; }
        public uint PartitionNumber { get; set; }
        public string DiskPath { get; set; }
        public ulong Offset { get; set; }
        public ulong Size { get; set; }
        public string SizeText { get { return TextFunctions.FormatFileSize(this.Size); } }
        public string DriveLetter { get; set; }
        public bool? Unallocated { get; set; }
        public bool? RecoveryPartition { get; set; }
        public ushort MbrType { get; set; }
        public string GptType { get; set; }

        public PartitionItem() { }

        public PartitionItem(ManagementObject wmi_storagePartition)
        {
            this.DiskNumber = (uint)wmi_storagePartition["DiskNumber"];
            this.PartitionNumber = (uint)wmi_storagePartition["PartitionNumber"];
            this.DiskPath = wmi_storagePartition["DiskId"] as string;
            this.Offset = (ulong)wmi_storagePartition["Offset"];
            this.Size = (ulong)wmi_storagePartition["Size"];
            this.DriveLetter = wmi_storagePartition["DriveLetter"] as string;
        }

        public static IEnumerable<PartitionItem> Load()
        {
            var wmi_storagePartition = new ManagementClass(@"\\.\root\Microsoft\Windows\Storage", "MSFT_Partition", new ObjectGetOptions()).GetInstances().OfType<ManagementObject>();

            return wmi_storagePartition.Select(x => new PartitionItem(x));
        }
    }
}
