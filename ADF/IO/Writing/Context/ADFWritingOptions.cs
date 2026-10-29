namespace Zion
{
    public sealed class ADFWritingOptions
    {
        public static readonly ADFWritingOptions Default = new();

        public int  MinPageSize   { get; init; } = 64 * 1024;
        public bool WriteHeader   { get; init; } = true;
        public bool Compression   { get; init; } = false;
        public bool CanWriteNull  { get; init; } = true;
    }
}