using System;
using System.Collections.Generic;
using System.Management;
using System.Text;
using Test436.Functions;

namespace DiskDriveManager.DiskDrive
{
    /// <summary>
    /// Disk information class
    /// [PowerShell example]
    /// $wmi_dsk = Get-CimInstance "Win32_DiskDrive"
    /// $wmi_str = Get-CimInstance -ClassName "MSFT_Disk" -Namespace "\\.\root\Microsoft\Windows\Storage"
    /// $wmi_phd = Get-CimInstance -ClassName "MSFT_PhysicalDisk" -Namespace "\\.\root\Microsoft\Windows\Storage"
    /// </summary>
    internal class DiskItem
    {
        #region Public parameter

        public uint Index { get; set; }
        public bool Online { get; set; }
        public string DeviceId { get; set; }
        public long Size { get; set; }
        public string SizeText { get { return TextFunctions.FormatFileSize(this.Size); } }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public PartitionStyle PartitionStyle { get; set; }
        public bool IsDynamicDisc { get; set; }
        public bool IsOffline { get; set; }
        public PhysicalDiskType PhysicalDiskType { get; set; }

        #endregion

        public DiskItem(ManagementObject wmi_diskdrive, ManagementObject wmi_storage, ManagementObject wmi_physicalDisks)
        {
            this.Index = (uint)wmi_diskdrive["Index"];
            this.DeviceId = wmi_diskdrive["DeviceID"] as string;
            this.Size = Convert.ToInt64((ulong)wmi_diskdrive["Size"]);
            this.SerialNumber = wmi_diskdrive["SerialNumber"] as string;
            this.Model = wmi_diskdrive["Model"] as string;
            (this.PartitionStyle, this.IsDynamicDisc) = CheckDiskStyle();
            this.IsOffline = (bool)wmi_storage["IsOffline"];
            this.PhysicalDiskType = (UInt16)wmi_physicalDisks["MediaType"] switch
            {
                0 => PhysicalDiskType.Unspecified,
                3 => PhysicalDiskType.HDD,
                4 => PhysicalDiskType.SSD,
                5 => PhysicalDiskType.SCM,
                _ => PhysicalDiskType.Unknown,
            };
        }

        public static IEnumerable<DiskItem> Load()
        {
            var wmi_diskdrives = new ManagementClass("Win32_DiskDrive").
                GetInstances().
                OfType<ManagementObject>();
            var wmi_storages = new ManagementClass(@"\\.\root\Microsoft\Windows\Storage", "MSFT_Disk", new ObjectGetOptions()).
                GetInstances().
                OfType<ManagementObject>();
            var wmi_physicaldisks = new ManagementClass(@"\\.\root\Microsoft\Windows\Storage", "MSFT_PhysicalDisk", new ObjectGetOptions()).
                GetInstances().
                OfType<ManagementObject>();

            return wmi_diskdrives.Select(x =>
                new DiskItem(x, 
                    wmi_storages.FirstOrDefault(y => (uint)x["Index"] == (uint)y["Number"]),
                    wmi_physicaldisks.FirstOrDefault(y => ((uint)x["Index"]).ToString() == y["DeviceId"] as string))).
                OrderBy(x => x.Index);
        }

        #region Dynamic/Basic, MBR/GPT disk checking

        // LDM related GPT partition GUID (metadata/data)
        private static readonly Guid LdmMetadataGuid = new Guid("e6d6d379-f507-44c2-a23c-238f2a3df928");
        private static readonly Guid LdmDataGuid = new Guid("af9b60a0-1431-4f62-bc68-3311714a69ad");

        private (PartitionStyle, bool) CheckDiskStyle()
        {
            //string path = $"\\\\.\\PhysicalDrive{diskIndex}";
            string path = this.DeviceId;
            const int sectorSize = 512;

            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //  Read sector 0 (MBR), MBR Signature Check
                    var sector0 = new byte[sectorSize];
                    if (fs.Read(sector0, 0, sector0.Length) != sector0.Length) return (PartitionStyle.Unknown, false);
                    if (sector0[510] != 0x55 || sector0[511] != 0xAA) return (PartitionStyle.Unknown, false);

                    bool hasProtective = false;
                    bool dynamicByMbr = false;

                    //  Check 4 entries in MBR
                    for (int i = 0; i < 4; i++)
                    {
                        int entryOffset = 446 + i * 16;
                        byte partType = sector0[entryOffset + 4];
                        if (partType == 0x42)       // MBR Dynamic (Microsoft Dynamic)
                        {
                            dynamicByMbr = true;
                        }
                        else if (partType == 0xEE) // Possibility of protective MBR -> GPT
                        {
                            hasProtective = true;
                        }
                    }

                    //  If there is no protective, MBR (dynamically determined by dynamicByMbr)
                    if (!hasProtective) return (PartitionStyle.MBR, dynamicByMbr);

                    //  Protective MBR present -> Read GPT header (LBA1)
                    fs.Seek(sectorSize, SeekOrigin.Begin);
                    var gptHeader = new byte[92];
                    if (fs.Read(gptHeader, 0, gptHeader.Length) != gptHeader.Length) return (PartitionStyle.Unknown, dynamicByMbr);

                    var sig = Encoding.ASCII.GetString(gptHeader, 0, 8);
                    if (sig != "EFI PART") return (PartitionStyle.Unknown, dynamicByMbr);

                    //  Get partition entry information from GPT header
                    ulong partEntryLba = BitConverter.ToUInt64(gptHeader, 72);
                    uint numEntries = BitConverter.ToUInt32(gptHeader, 80);
                    uint sizeEntry = BitConverter.ToUInt32(gptHeader, 84);

                    //  Move to the beginning of the entries and traverse
                    long entriesOffset = (long)partEntryLba * sectorSize;
                    fs.Seek(entriesOffset, SeekOrigin.Begin);

                    var entryBuf = new byte[sizeEntry];
                    for (uint i = 0; i < numEntries; i++)
                    {
                        int read = fs.Read(entryBuf, 0, entryBuf.Length);
                        if (read != entryBuf.Length) break;

                        //  The first 16 bytes are the partition type GUID (raw bytes)
                        var guidBytes = new byte[16];
                        Array.Copy(entryBuf, 0, guidBytes, 0, 16);

                        try
                        {
                            var typeGuid = new Guid(guidBytes);

                            //  Dynamically find the LDM metadata/data GUID
                            if (typeGuid == LdmMetadataGuid || typeGuid == LdmDataGuid)
                            {
                                return (PartitionStyle.GPT, true);
                            }
                        }
                        catch (ArgumentException) { }
                    }

                    //  GPT but no LDM GUID found -> GPT and basic
                    return (PartitionStyle.GPT, false);
                }
            }
            catch (UnauthorizedAccessException) { return (PartitionStyle.Unknown, false); }
            catch (IOException) { return (PartitionStyle.Unknown, false); }
            catch (ArgumentException) { return (PartitionStyle.Unknown, false); }
        }

        #endregion
    }
}
