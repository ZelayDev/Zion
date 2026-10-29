namespace Zion.ADF
{
    internal abstract class StructWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public StructWriteStrategy(uint FormatId) : base(FormatId) { }
    }
}