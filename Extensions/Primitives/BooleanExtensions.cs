using Zion.Serialization;

namespace Zion
{
    public static class BooleanExtensions
    {
        public static IBinarySerializer<bool> _Serializer = new BinarySerializer<bool>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadBoolean()
        );

        extension(bool Value)
        {
            public static IBinarySerializer<bool> Serializer => _Serializer;
        }
    }
}