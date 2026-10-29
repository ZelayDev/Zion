using Zion.Serialization;

namespace Zion
{
    public static class SByteExtensions
    {
        public static IBinarySerializer<sbyte> _Serializer = new BinarySerializer<sbyte>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadSByte()
        );

        extension(sbyte Value)
        {
            public static IBinarySerializer<sbyte> Serializer => _Serializer;


            /// <summary>
            /// Gets the state of a specific bit in the sbyte.
            /// </summary>
            /// <param name="Value">The sbyte value.</param>
            /// <param name="Index">The bit position (0-7).</param>
            /// <returns>True if the bit is set, otherwise false.</returns>
            public bool GetBit(int Index)
            {
                return (Value & (1 << Index)) != 0;
            }

            /// <summary>
            /// Sets or clears a specific bit in the sbyte.
            /// </summary>
            /// <param name="Value">The sbyte value.</param>
            /// <param name="Index">The bit position (0-7).</param>
            /// <param name="Bit">True to set the bit, false to clear it.</param>
            /// <returns>The modified sbyte value.</returns>
            public sbyte SetBit(int Index, bool Bit)
            {
                return Bit ? (sbyte)(Value | (sbyte)(1 << Index)) : (sbyte)(Value & ~(1 << Index));
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
            /// Checks if the sbyte value is even.
            /// </summary>
            /// <param name="Value">The sbyte value.</param>
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

                sbyte MaxValue = (sbyte)Math.Sqrt(Value);

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