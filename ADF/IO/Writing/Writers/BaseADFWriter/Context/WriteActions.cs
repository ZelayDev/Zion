namespace Zion.ADF
{
    internal delegate void StreamWriteAction<T>(ArenaStream Stream, ReadOnlySpan<T> Span, bool Compression);

    internal delegate void SequenceWriteAction<T>(ADFWritingContext Context, StreamGroup Target, object? Collection, ReadOnlySpan<T> Span);
}