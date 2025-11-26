using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Management;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DiskDriveManager.DiskDrive
{
    internal class DiskDriveHelper
    {
        public static DiskItem[] GetInfo()
        {
            var disks = DiskItem.Load().ToArray();
            var partitions = PartitionItem.Load().ToArray();
            var drives = DriveItem.Load().ToArray();

            //  link partition <=> drive
            LinkPartitionToDrive(partitions, drives);

            //  merge partitions with unallocated spaces
            for (int i = 0; i < disks.Length; i++)
            {
                disks[i].Partitions = MergePartitionsAndUnallocated(disks[i], partitions);
            }
            return disks;
        }

        private static void LinkPartitionToDrive(PartitionItem[] partitions, DriveItem[] drives)
        {
            var wmi_partitionToDrive = new ManagementClass(@"\\.\root\Microsoft\Windows\Storage", "MSFT_PartitionToVolume", new ObjectGetOptions()).GetInstances().OfType<ManagementObject>();
            foreach (var link in wmi_partitionToDrive)
            {
                string partitionId = null;
                var partRef = link["Partition"];
                if (partRef is string pathPart)
                {
                    partitionId = new ManagementObject(pathPart)["ObjectId"] as string;
                }

                string driveId = null;
                var volRef = link["Volume"];
                if (volRef is string pathVol)
                {
                    driveId = new ManagementObject(pathVol)["ObjectId"] as string;
                }

                var partition = partitions.FirstOrDefault(x => x.ObjectId == partitionId);
                var drive = drives.FirstOrDefault(x => x.ObjectId == driveId);
                if (partition != null && drive != null)
                {
                    partition.Drive = drive;
                    drive.DiskNumber = partition.DiskNumber;
                    drive.PartitionNumber = partition.PartitionNumber;
                }
            }
        }

        private static PartitionItem[] MergePartitionsAndUnallocated(DiskItem disk, IEnumerable<PartitionItem> partitions)
        {
            if (disk == null) throw new ArgumentNullException(nameof(disk));
            var parts = (partitions ?? Array.Empty<PartitionItem>()).
                Where(x => x.DiskNumber == disk.DiskNumber).
                OrderBy(x => x.Offset).
                ToArray();
            foreach (var part in parts)
            {
                part.Unallocated = false;
            }
            var maerged = new List<PartitionItem>(parts);

            //  If the disk size is unknown or 0, only the actual partitions are returned.
            if (disk.Size == 0) return maerged.OrderBy(x => x.Offset).ToArray();

            //  If there are no partitions, add the entire disk as unallocated.
            if (!parts.Any())
            {
                maerged.Add(new PartitionItem()
                {
                    DiskNumber = disk.DiskNumber,
                    PartitionNumber = 0,
                    Unallocated = true,
                    DiskPath = disk.DiskPath,
                    Offset = 0,
                    Size = disk.Size,
                });
                return maerged.OrderBy(x => x.Offset).ToArray();
            }

            //  Leading Gap
            if (parts[0].Offset > 0)
            {
                maerged.Add(new PartitionItem()
                {
                    DiskNumber = disk.DiskNumber,
                    PartitionNumber = 0,
                    Unallocated = true,
                    DiskPath = disk.DiskPath,
                    Offset = 0,
                    Size = parts[0].Offset,
                });
            }

            //  Inter-partition Gap
            //  (Do not add unassigned if overlapping or connected)
            for (int i = 1; i < parts.Length; i++)
            {
                var prev = parts[i - 1];
                var cur = parts[i];
                var prevEnd = prev.Offset + prev.Size;
                if (cur.Offset > prevEnd)
                {
                    maerged.Add(new PartitionItem()
                    {
                        DiskNumber = disk.DiskNumber,
                        PartitionNumber = 0,
                        Unallocated = true,
                        DiskPath = disk.DiskPath,
                        Offset = prevEnd,
                        Size = cur.Offset - prevEnd,
                    });
                }
            }

            //  Tail Gap
            var last = parts.Last();
            ulong lastend = last.Offset + last.Size;
            if (lastend < disk.Size)
            {
                maerged.Add(new PartitionItem()
                {
                    DiskNumber = disk.DiskNumber,
                    PartitionNumber = 0,
                    Unallocated = true,
                    DiskPath = disk.DiskPath,
                    Offset = lastend,
                    Size = disk.Size - lastend,
                });
            }

            return maerged.OrderBy(x => x.Offset).ToArray();
        }



    }
}
