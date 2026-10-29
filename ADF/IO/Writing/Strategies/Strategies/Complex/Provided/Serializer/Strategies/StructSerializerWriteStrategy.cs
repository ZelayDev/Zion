namespace Zion.ADF
{
    internal sealed class StructSerializerWriteStrategy<T> : StructWriteStrategy<T>
    {
        private readonly IADFSerializer<T> Serializer;

        public StructSerializerWriteStrategy(uint FormatId, IADFSerializer<T> Serializer) : base(FormatId)
        {
            this.Serializer = Serializer.NotNull();
        }

        protected override void Write(ADFWritingContext Context, StreamGroup Base, ADFObjectWriter Writer, T Value)
        {
            Serializer.Write(Writer, Value);
        }
    }
}