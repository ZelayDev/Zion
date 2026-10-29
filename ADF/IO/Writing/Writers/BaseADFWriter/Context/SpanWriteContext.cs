namespace Zion.ADF
{
    internal ref struct SpanWriteContext<T>
    {
        public delegate void WriteAction(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<T> Span);

        public readonly string Name;
        public readonly uint   FormatId;
        public readonly object? Collection;

        public readonly ReadOnlySpan<T> Span;        
        public readonly WriteAction Write;


        public SpanWriteContext(string Name, uint FormatId, object? Collection, ReadOnlySpan<T> Span, WriteAction Writing)
        {
            this.Name = Name.NotNull();
            this.FormatId = FormatId | ADFPrimitives.Sequence;

            this.Collection = Collection.NotNull();
            this.Span = Span;

            this.Write = Writing.NotNull();
        }
    }
}