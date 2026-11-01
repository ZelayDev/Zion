using System.Numerics;
using Zion.Serialization;

namespace Zion
{
    public static class BigIntegerExtnesions
    {
        private static readonly IBinarySerializer<BigInteger> _Serializer = new BinarySerializer<BigInteger>
        (
            static (Writer, Value) =>
            {
                var Length = Value.GetByteCount();
                Span<byte> Buffer = stackalloc byte[Length];
                Value.TryWriteBytes(Buffer, out var Writed);

                Writer.Write(Length);
                Writer.Write(Buffer);
            },
            static Reader =>
            {
                var Length = Reader.ReadInt32();
                Span<byte> Buffer = stackalloc byte[Length];
                Reader.BaseStream.ReadExactly(Buffer);
                return new BigInteger(Buffer);
            }
        );

        extension(BigInteger Value)
        {
            public static IBinarySerializer<BigInteger> Serializer => _Serializer;
        }
    }
}