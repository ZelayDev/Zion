namespace Zion.ADF
{
    internal abstract class ClassWriteStrategy<T> : ProvidedWriteStrategy<T>
    {
        public ClassWriteStrategy(uint FormatId) : base(FormatId) { }


        protected sealed override void Write(ADFWritingContext Context, StreamGroup Base, ADFObjectWriter Writer, T Value)
        {
            var Link = Reference.CreateNewReference(Base.ChildsLength);
            Base.BaseStream.WriteCompressed(Context, Link);
            WriteValue(Writer, Value);
        }

        protected sealed override void OnWrited(ADFWritingContext Context, uint FormatId, T Value)
        {
            var Definition = new DataDefinition
            (
                FormatId,
                Context.CurrentPage,
                Context.CurrentPosition
            );

            Context.Registries.References.Add(Value!, Definition);
        }


        protected abstract void WriteValue(ADFObjectWriter Writer, T Value);
    }
}