namespace DiskDriveManager.Functions.EnumParser
{
    internal class DriveTypeParser : ParserBase<DriveType>
    {
        public DriveTypeParser()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            map = new()
            {
                { new string[] { "Unknown", "0" }, DriveType.Unknown },
                { new string[] { "NoRootDirectory", "No Root Directory", "InvalidRootPath", "Invalid Root Path", "1" }, DriveType.NoRootDirectory },
                { new string[] { "Removable", "Removable Disk", "2" }, DriveType.Removable },
                { new string[] { "Fixed", "LocalDisk", "Local Disk", "3" }, DriveType.Fixed },
                { new string[] { "Network", "Network Disk", "NetworkDisk", "Remote", "4" }, DriveType.Network },
                { new string[] { "CDRom", "CD-ROM", "Compact Disc", "5" }, DriveType.CDRom },
                { new string[] { "Ram", "Ram Disk", "RamDisk", "6" }, DriveType.Ram },
            };
        }

        #region Static methods.

        private static DriveTypeParser _parser = null;

        public static uint ParamsToRaw(DriveType flags)
        {
            _parser ??= new DriveTypeParser();
            return (uint)_parser.FlagsToNumber(flags);
        }

        public static DriveType RawToParam(uint num)
        {
            _parser ??= new DriveTypeParser();
            return _parser.NumberToFlags(Convert.ToInt32(num));
        }

        #endregion
    }
}
