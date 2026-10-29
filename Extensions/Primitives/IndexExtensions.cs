using Zion.Serialization;

namespace Zion
{
    public static class IndexExtensions
    {
        private static readonly IBinarySerializer<Index> _Serializer = new BinarySerializer<Index>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadIndex()
        );

        extension(Index Value)
        {
            public static IBinarySerializer<Index> Serializer => _Serializer;
        }
    }
}