using System.Runtime.InteropServices;

namespace Zion.ADF
{
    internal static class ListExtensions
    {
        extension<T>(List<T> List)
        {
            //TODO: Check SetCount
            public Span<T> AsSpan()
            {
                return CollectionsMarshal.AsSpan(List);
            }

            public Span<T> AsSpan(int Start)
            {
                return CollectionsMarshal.AsSpan(List).Slice(Start);
            }

            public Span<T> AsSpan(int Start, int Count)
            {
                return CollectionsMarshal.AsSpan(List).Slice(Start, Count);
            }
        }
    }
}