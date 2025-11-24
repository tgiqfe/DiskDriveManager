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
        public long Size { get; set; }
        public string SizeText { get { return TextFunctions.FormatFileSize(this.Size); } }
        public string DriveLetter { get; set; }

        public PartitionItem(ManagementObject wmi_pysicalpartition, ManagementObject wmi_diskpartition)
        {
            this.DiskNumber = (uint)wmi_pysicalpartition["DiskNumber"];
            this.PartitionNumber = (uint)wmi_pysicalpartition["PartitionNumber"];
            this.DiskPath = wmi_pysicalpartition["DiskId"] as string;
        }

        public static PartitionItem[] Load()
        {
            var wmi_pysicalpartitions = new ManagementClass(@"\\.\root\Microsoft\Windows\Storage", "MSFT_Partition", new ObjectGetOptions()).GetInstances().OfType<ManagementObject>();
            var wmi_diskpartitions = new ManagementClass("Win32_DiskPartition").GetInstances().OfType<ManagementObject>();

            return null;
        }
    }
}
