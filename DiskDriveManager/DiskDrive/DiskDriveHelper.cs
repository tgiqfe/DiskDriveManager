using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DiskDriveManager.DiskDrive
{
    internal class DiskDriveHelper
    {
        public static DiskItem[] GetInfo()
        {
            var disks = DiskItem.Load().ToArray();
            var partitions = PartitionItem.Load();
            for (int i = 0; i < disks.Length; i++)
            {
                disks[i].Partitions = MergePartitionsAndUnallocated(disks[i], partitions);
            }
            return disks;
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
                    DriveLetter = null
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
                    DriveLetter = null
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
                        DriveLetter = null
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
                    DriveLetter = null
                });
            }

            return maerged.OrderBy(x => x.Offset).ToArray();
        }
    }
}
