namespace Zion.ADF
{
    internal sealed class SerializableClassWriteStrategy<T> : ClassWriteStrategy<T>
    {
        public SerializableClassWriteStrategy(uint FormatId) : base(FormatId)
        {
            if (!typeof(T).IsAssignableFrom(typeof(IADFWritable)))
            {
                throw new InvalidCastException($"{typeof(T)} not realized {typeof(IADFWritable)}");
            }
        }

        protected override void WriteValue(ADFObjectWriter Writer, T Value)
        {
            var Writable = ((IADFWritable)Value!).NotNull();
            Writable.Write(Writer);
        }
    }
}