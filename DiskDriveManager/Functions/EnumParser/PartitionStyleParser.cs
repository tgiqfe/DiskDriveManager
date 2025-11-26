namespace DiskDriveManager.Functions.EnumParser
{
    internal class PartitionStyleParser : ParserBase<PartitionStyle>
    {
        public PartitionStyleParser()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            map = new()
            {
                { new string[] { "Unknown", "Raw", "0" }, PartitionStyle.Unknown },
                { new string[] { "MBR", "Master Boot Record", "MasterBootRecord", "1" }, PartitionStyle.MBR },
                { new string[] { "GPT", "GUID Partition Table", "GuidPartitionTable", "2" }, PartitionStyle.GPT },
            };
        }

        #region Static methods.

        private static PartitionStyleParser _parser = null;

        public static uint ParamsToRaw(PartitionStyle flags)
        {
            _parser ??= new PartitionStyleParser();
            return (uint)_parser.FlagsToNumber(flags);
        }

        public static PartitionStyle RawToParam(uint num)
        {
            _parser ??= new PartitionStyleParser();
            return _parser.NumberToFlags(Convert.ToInt32(num));
        }

        #endregion
    }

    internal enum PartitionStyle
    {
        Unknown = 0,
        MBR = 1,
        GPT = 2,
    }
}
