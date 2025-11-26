using DiskDriveManager.Functions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Management;
using System.Text;
using System.Text.Json.Serialization;

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
        #region Public parameter

        public uint DiskNumber { get; set; }
        public uint PartitionNumber { get; set; }
        public string DiskPath { get; set; }
        public ulong Offset { get; set; }
        public ulong Size { get; set; }
        public string SizeText { get { return TextFunctions.FormatFileSize(this.Size); } }
        public string DriveLetter { get; set; }
        public bool RecoveryPartition { get; set; }

        // Setting with DiskDriveHelper
        public bool? Unallocated { get; set; }
        public DriveItem Drive { get; set; }

        [JsonIgnore]
        public string ObjectId { get; private set; }

        #endregion

        public PartitionItem() { }

        public PartitionItem(ManagementObject wmi_partition)
        {
            this.DiskNumber = (uint)wmi_partition["DiskNumber"];
            this.PartitionNumber = (uint)wmi_partition["PartitionNumber"];
            this.DiskPath = wmi_partition["DiskId"] as string;
            this.Offset = (ulong)wmi_partition["Offset"];
            this.Size = (ulong)wmi_partition["Size"];
            this.DriveLetter = wmi_partition["DriveLetter"] as string;
            this.RecoveryPartition = IsRecoveryPartition(wmi_partition);
            this.ObjectId = wmi_partition["ObjectId"] as string;
        }

        public static IEnumerable<PartitionItem> Load()
        {
            var wmi_partitions = new ManagementClass(@"\\.\root\Microsoft\Windows\Storage", "MSFT_Partition", new ObjectGetOptions()).GetInstances().OfType<ManagementObject>();

            return wmi_partitions.Select(x => new PartitionItem(x));
        }

        #region Recovery Partition Checking

        private static readonly string[] RecoveryGptGuids = new string[]
        {
            "DE94BBA4-06D1-4D40-A16A-BFD50179D6AC"
        };
        private static readonly int[] RecoveryMbrTypes = new int[]
        {
            0x27
        };

        private bool IsRecoveryPartition(ManagementObject wmi_partition)
        {
            try
            {
                // Check GPT type Recovery Partition GUID
                Func<string, bool> checkGptType = (text) =>
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        var guidText = text.Trim().Trim('{', '}');
                        return RecoveryGptGuids.Any(x => x.Equals(guidText, StringComparison.OrdinalIgnoreCase));
                    }
                    return false;
                };
                var ret = checkGptType(wmi_partition["GptType"] as string);
                if (ret) return true;
                ret = checkGptType(wmi_partition["GptTypeGuid"] as string);
                if (ret) return true;

                //  Check MBR type Recovery Partition Type
                Func<object, bool> checkMbrType = (obj) =>
                {
                    if (obj != null)
                    {
                        var s = obj.ToString();
                        if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                        {
                            var num = Convert.ToInt32(s.Substring(2), 16);
                            if (RecoveryMbrTypes.Any(x => x == num)) return true;
                        }
                        else if (int.TryParse(s, out int num))
                        {
                            if (RecoveryMbrTypes.Any(x => x == num)) return true;
                        }
                        else
                        {
                            if (obj is byte b && RecoveryMbrTypes.Any(x => x == b)) return true;
                            if (obj is int i && RecoveryMbrTypes.Any(x => x == i)) return true;
                        }
                    }
                    return false;
                };
                ret = checkMbrType(wmi_partition["MbrType"]);
                if (ret) return true;
                ret = checkMbrType(wmi_partition["Type"]);
                if (ret) return true;
            }
            catch { }
            return false;
        }

        #endregion
    }
}
