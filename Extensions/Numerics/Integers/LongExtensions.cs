using Zion.Serialization;

namespace Zion
{
    public static class LongExtensions
    {
        public static IBinarySerializer<long> _Serializer = new BinarySerializer<long>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadInt64()
        );

        extension(long Value)
        {
            public static IBinarySerializer<long> Serializer => _Serializer;


            /// <summary>
            /// Gets the state of a specific bit in the long integer.
            /// </summary>
            /// <param name="Value">The long integer value.</param>
            /// <param name="Index">The bit position (0-63).</param>
            /// <returns>True if the bit is set, otherwise false.</returns>
            public bool GetBit(int Index)
            {
                return (Value & (1L << Index)) != 0;
            }

            /// <summary>
            /// Sets or clears a specific bit in the long integer.
            /// </summary>
            /// <param name="Value">The long integer value.</param>
            /// <param name="Index">The bit position (0-63).</param>
            /// <param name="Bit">True to set the bit, false to clear it.</param>
            /// <returns>The modified long integer value.</returns>
            public long SetBit(int Index, bool Bit)
            {
                return Bit ? Value | (1L << Index) : Value & ~(1L << Index);
            }


            public IEnumerable<bool> EnumerateBits()
            {
                return EnumerateBits(64);
            }

            public IEnumerable<bool> EnumerateBits(int Count)
            {
                long CurrentBit = 1L;
                for (int i = 0; i < Count; i++)
                {
                    yield return (Value & CurrentBit) != 0;
                    CurrentBit <<= 1;
                }
            }


            /// <summary>
            /// Checks if the long integer value is even.
            /// </summary>
            /// <param name="Value">The long integer value.</param>
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

                long MaxValue = (long)Math.Sqrt(Value);

                for (long i = 3; i <= MaxValue; i += 2)
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