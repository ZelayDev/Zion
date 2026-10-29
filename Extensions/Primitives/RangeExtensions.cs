using Zion.Serialization;

namespace Zion
{
    public static class RangeExtensions
    {
        private static readonly IBinarySerializer<Range> _Serializer = new BinarySerializer<Range>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadRange()
        );

        extension(Range Value)
        {
            public static IBinarySerializer<Range> Serializer => _Serializer;
        }
    }
}