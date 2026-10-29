namespace Zion.ADF
{
    internal sealed class ClassSerializerWriteStrategy<T> : ClassWriteStrategy<T>
    {
        private readonly IADFSerializer<T> Serializer;

        public ClassSerializerWriteStrategy(uint FormatId, IADFSerializer<T> Serializer) : base(FormatId)
        {
            this.Serializer = Serializer;
        }

        protected override void WriteValue(ADFObjectWriter Writer, T Value)
        {
            Serializer.Write(Writer, Value);
        }
    }
}