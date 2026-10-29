namespace Zion.ADF
{
    internal readonly record struct WriteEntry(uint FormatId, IWriteStrategy Strategy);

    internal readonly record struct WriteEntry<T>(uint FormatId, IWriteStrategy<T> Strategy)
    {
        public static implicit operator WriteEntry<T>(WriteEntry Entry)
        {
            return new(Entry.FormatId, (IWriteStrategy<T>)Entry.Strategy);
        }

        public static implicit operator WriteEntry(WriteEntry<T> Entry)
        {
            return new(Entry.FormatId, Entry.Strategy);
        }
    }
}