using System;
using System.Collections.Generic;
using System.Text;

namespace DiskDriveManager.DiskDrive
{
    internal class DriveTypeParser
    {
        public static string ParseDriveType(DriveType driveType)
        {
            return driveType switch
            {
                DriveType.Unknown => "Unknown",
                DriveType.NoRootDirectory => "No Root Directory",
                DriveType.Removable => "Removable Disk",
                DriveType.Fixed => "Local Disk",
                DriveType.Network => "Network Drive",
                DriveType.CDRom => "Compact Disc",
                DriveType.Ram => "RAM Disk",
                _ => "Invalid Drive Type"
            };
        }
    }
}
