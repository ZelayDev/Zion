using System.Runtime.InteropServices;

namespace Zion.ADF
{
    internal static class ListExtensions
    {
        extension<T>(List<T> List)
        {
            public ReadOnlySpan<T> AsSpan()
            {
                return CollectionsMarshal.AsSpan(List);
            }

            public ReadOnlySpan<T> AsSpan(int Start)
            {
                return CollectionsMarshal.AsSpan(List).Slice(Start);
            }

            public ReadOnlySpan<T> AsSpan(int Start, int Count)
            {
                return CollectionsMarshal.AsSpan(List).Slice(Start, Count);
            }
        }
    }
}