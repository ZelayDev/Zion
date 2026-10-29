namespace Zion.ADF
{
    public sealed class ADFMixedLayersWriter<T> : ADFCheckingObjectWriter
    {
        //TODO: Для первого раза добавить HashSet UsedNames для проверки.
        private readonly ILayerWriteInfo[] Layers;
        private readonly uint FormatId;


        internal ADFMixedLayersWriter(ADFWritingContext Context, StreamGroup Target, ILayerWriteInfo[] Layers, uint FormatId)
            : base(Context, Target, default)
        {
            this.Layers = Layers;
            this.FormatId = FormatId;
        }


        protected override void OnDisposed() { }


        internal void Serialize(T Value)
        {
            WriteSerializers(Value);
            Dispose();
        }


        private void WriteSerializers(T Value)
        {
            ADFRecordObjectWriter? RecordWriter = null;

            var FormatRegistry = this.FormatRegistry;
            var FormatId = this.FormatId;
            var Context = this.Context;
            var Target = Data;

            foreach (var Layer in Layers)
            {
                var Format = FormatRegistry[FormatId];
                FormatId = Format.BaseFormat;

                if (Layer is AutoType<T> Auto)
                {
                    Auto.Writer.Invoke(Context, Target, Value);
                    continue;
                }
                else if (Layer is IADFSerializer<T> Manual)
                {
                    if (Format.IsDeferred)
                    {
                        RecordWriter ??= new ADFRecordObjectWriter(Context, Target);
                        Manual.Write(RecordWriter, Value);

                        FormatRegistry.Clarify(FormatId, RecordWriter);
                        RecordWriter.Reset();
                    }
                    else
                    {
                        this.Format = Format;

                        Reset();
                        Manual.Write(this, Value);
                        ValidateLayer();
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Unknow LayerInfo: {Layer.GetType()}");
                }
            }
        }
    }
}