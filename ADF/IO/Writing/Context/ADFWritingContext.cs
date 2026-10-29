namespace Zion.ADF
{
    internal sealed class ADFWritingContext
    {
        public readonly Arena<byte>        Arena;
        public readonly ADFWritingOptions  Options;
        public readonly WritableRegistries Registries;
        public readonly WriteStrategies    WriteStrategies;

        public readonly bool Compression;

        public uint CurrentPage;
        public long CurrentPosition;

        public ADFWritingContext(ADFWritingOptions? WritingOptions)
        {
            Options = WritingOptions ?? ADFWritingOptions.Default;
            Compression = Options.Compression;
            Arena           = new(GetArenaCapacity(Options.MinPageSize));
            Registries      = new();
            WriteStrategies = new();
        }


        private static int GetArenaCapacity(int MinPageSize)
        {
            return MinPageSize + (MinPageSize >> 1);
        }
    }
}