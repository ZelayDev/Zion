namespace Zion.ADF
{
    internal sealed class ADFManualLayersWriter<T> : ADFCheckingObjectWriter
    {
        private readonly IADFSerializer<T>[] Serializers;
        private readonly uint FormatId;
        private bool IsFirstWriting;


        internal ADFManualLayersWriter(ADFWritingContext Context, StreamGroup Target, IADFSerializer<T>[] Serializers, uint FormatId)
            : base(Context, Target, default)
        {
            var FirstFormat = FormatRegistry[FormatId];

            this.IsFirstWriting = FirstFormat.IsDeferred;
            this.Serializers = Serializers;
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
            if (IsFirstWriting)
            {
                WriteToRecord(Value);
                IsFirstWriting = false;
            }
            else
            {
                WriteToSelf(Value, FormatId);
            }
        }

        private void WriteToRecord(T Value)
        {
            using var Writer = new ADFRecordObjectWriter(Context, Data);

            var FormatRegistry = this.FormatRegistry;
            var Serializers    = this.Serializers;
            var FormatId       = this.FormatId;

            for (int i = 0; i < Serializers.Length; i++)
            {
                var Format = FormatRegistry[FormatId];

                if (!Format.IsDeferred)
                {
                    WriteToSelf(Value, FormatId, i);
                    break;
                }

                Serializers[i].Write(Writer, Value);
                FormatRegistry.Clarify(FormatId, Writer);
                Writer.Reset();

                FormatId = Format.BaseFormat;
            }
        }

        private void WriteToSelf(T Value, uint FormatId, int Index = 0)
        {
            var FormatRegistry = this.FormatRegistry;
            var Serializers = this.Serializers;

            for (int i = Index; i < Serializers.Length; i++)
            {
                var Format = FormatRegistry[FormatId];
                FormatId = Format.BaseFormat;

                this.Format = Format;

                Reset();
                Serializers[i].Write(this, Value);
                ValidateLayer();
            }
        }
    }
}