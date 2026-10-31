using System.Runtime.CompilerServices;

namespace Zion
{
    public static class ArenaSpanExtensions
    {
        extension<T>(Span<T> Span)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void CopyTo(ArenaSpan<T> Destination)
            {
                Destination.CopyFrom(Span);
            }
        }
    }
}