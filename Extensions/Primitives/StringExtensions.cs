using Zion.Serialization;

namespace Zion
{
    public static class StringExtensions
    {
        public static IBinarySerializer<string> _Serializer = new BinarySerializer<string>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadString()
        );

        extension(string Value)
        {
            public static IBinarySerializer<string> Serializer => _Serializer;
        }
    }
}