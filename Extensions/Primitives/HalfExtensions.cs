using Zion.Serialization;

namespace Zion
{
    public static class HalfExtensions
    {
        private static readonly IBinarySerializer<Half> _Serializer = new BinarySerializer<Half>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadHalf()
        );

        extension(Half Value)
        {
            public static IBinarySerializer<Half> Serializer => _Serializer;
        }
    }
}