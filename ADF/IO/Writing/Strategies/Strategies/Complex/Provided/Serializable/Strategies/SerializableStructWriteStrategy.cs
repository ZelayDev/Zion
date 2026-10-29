namespace Zion.ADF
{
    internal sealed class SerializableStructWriteStrategy<T> : StructWriteStrategy<T>
    {
        public SerializableStructWriteStrategy(uint FormatId) : base(FormatId)
        {
            if (!typeof(T).IsAssignableFrom(typeof(IADFWritable)))
            {
                throw new InvalidCastException($"{typeof(T)} not realized {typeof(IADFWritable)}");
            }
        }


        protected override void Write(ADFWritingContext Context, StreamGroup Base, ADFObjectWriter Writer, T Value)
        {
            var Writable = ((IADFWritable)Value!).NotNull();
            Writable.Write(Writer);
        }
    }
}