using Zion.Serialization;

namespace Zion
{
    public static class ByteExtensions
    {
        public static IBinarySerializer<byte> _Serializer = new BinarySerializer<byte>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadByte()
        );

        extension(byte Value)
        {
            public static IBinarySerializer<byte> Serializer => _Serializer;


            /// <summary>
            /// Gets the state of a specific bit in the byte.
            /// </summary>
            /// <param name="Value">The byte value.</param>
            /// <param name="Index">The bit position (0-7).</param>
            /// <returns>True if the bit is set, otherwise false.</returns>
            public bool GetBit(int Index)
            {
                return (Value & (1 << Index)) != 0;
            }

            /// <summary>
            /// Sets or clears a specific bit in the byte.
            /// </summary>
            /// <param name="Value">The byte value.</param>
            /// <param name="Index">The bit position (0-7).</param>
            /// <param name="Bit">True to set the bit, false to clear it.</param>
            /// <returns>The modified byte value.</returns>
            public byte SetBit(int Index, bool Bit)
            {
                return Bit ? (byte)(Value | (1 << Index)) : (byte)(Value & ~(1 << Index));
            }

            
            public IEnumerable<bool> EnumerateBits()
            {
                yield return (Value & 0b0000_0001) != 0;
                yield return (Value & 0b0000_0010) != 0;
                yield return (Value & 0b0000_0100) != 0;
                yield return (Value & 0b0000_1000) != 0;
                yield return (Value & 0b0001_0000) != 0;
                yield return (Value & 0b0010_0000) != 0;
                yield return (Value & 0b0100_0000) != 0;
                yield return (Value & 0b1000_0000) != 0;
            }

            public IEnumerable<bool> EnumerateBits(int Count)
            {
                int CurrentBit = 1;
                for (int i = 0; i < Count; i++)
                {
                    yield return (Value & CurrentBit) != 0;
                    CurrentBit <<= 1;
                }
            }


            /// <summary>
            /// Checks if the byte value is even.
            /// </summary>
            /// <param name="Value">The byte value.</param>
            /// <returns>True if the value is even, otherwise false.</returns>
            public bool IsEven()
            {
                return (Value & 1L) == 0;
            }

            public bool IsPrime()
            {
                if (Value <= 1L) { return false; }
                if (Value == 2) { return true; }
                if (IsEven(Value)) { return false; }

                byte MaxValue = (byte)Math.Sqrt(Value);

                for (int i = 3; i <= MaxValue; i += 2)
                {
                    if (Value % i == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}