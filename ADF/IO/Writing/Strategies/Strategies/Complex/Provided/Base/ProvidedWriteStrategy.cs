namespace Zion.ADF
{
    internal abstract class ProvidedWriteStrategy<T> : IWriteStrategy<T>
    {
        private readonly uint FormatId;
        private bool HasFormat;
        private DataFormat Format;


        public ProvidedWriteStrategy(uint FormatId)
        {
            this.FormatId = FormatId;
        }


        public static ProvidedWriteStrategy<T> GetStrategy(uint FormatId)
        {
            return typeof(T).IsValueType
                ? new SerializableStructWriteStrategy<T>(FormatId)
                : new SerializableClassWriteStrategy<T>(FormatId);
        }

        public static ProvidedWriteStrategy<T> GetStrategy(uint FormatId, IADFSerializer<T> Serializer)
        {
            return typeof(T).IsValueType
                ? new StructSerializerWriteStrategy<T>(FormatId, Serializer)
                : new ClassSerializerWriteStrategy<T>(FormatId, Serializer);
        }


        protected abstract void Write(ADFWritingContext Context, StreamGroup Base, ADFObjectWriter Writer, T Value);

        protected virtual void OnWrited(ADFWritingContext Context, uint FormatId, T Value) { }


        public void WriteData(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            if (HasFormat)
            {
                using var Writer = new ADFCheckingObjectWriter(Context, Target, Format);
                Write(Context, Target, Writer, Value);
                OnWrited(Context, FormatId, Value);
            }
            else
            {
                using var Writer = new ADFRecordObjectWriter(Context, Target);
                Write(Context, Target, Writer, Value);

                HasFormat = true;
                Format = Context.Registries.FormatRegistry.Clarify(FormatId, Writer);

                OnWrited(Context, FormatId, Value);
            }
        }
    }
}