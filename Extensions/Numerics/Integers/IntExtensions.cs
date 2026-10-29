using Zion.Serialization;

namespace Zion
{
    public static class IntExtensions
    {
        public static IBinarySerializer<int> _Serializer = new BinarySerializer<int>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadInt32()
        );

        extension(int Value)
        {
            public static IBinarySerializer<int> Serializer => _Serializer;


            /// <summary>
            /// Gets the state of a specific bit in the integer.
            /// </summary>
            /// <param name="Value">The integer value.</param>
            /// <param name="Index">The bit position (0-31).</param>
            /// <returns>True if the bit is set, otherwise false.</returns>
            public bool GetBit(int Index)
            {
                return (Value & (1 << Index)) != 0;
            }

            /// <summary>
            /// Sets or clears a specific bit in the integer.
            /// </summary>
            /// <param name="Value">The integer value.</param>
            /// <param name="Index">The bit position (0-31).</param>
            /// <param name="Bit">True to set the bit, false to clear it.</param>
            /// <returns>The modified integer value.</returns>
            public int SetBit(int Index, bool Bit)
            {
                return Bit ? Value | (1 << Index) : Value & ~(1 << Index);
            }


            public IEnumerable<bool> EnumerateBits()
            {
                return EnumerateBits(32);
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
            /// Checks if the integer value is even.
            /// </summary>
            /// <param name="Value">The integer value.</param>
            /// <returns>True if the value is even, otherwise false.</returns>
            public bool IsEven()
            {
                return (Value & 1) == 0;
            }

            public bool IsPrime()
            {
                if (Value <= 1) { return false; }
                if (Value == 2) { return true; }
                if (IsEven(Value)) { return false; }

                int MaxValue = (int)Math.Sqrt(Value);

                for (int i = 3; i <= MaxValue; i += 2)
                {
                    if (Value % i == 0)
                    {
                        return false;
                    }
                }

                return true;
            }


            public bool IsWithin<T>(ICollection<T> Collection)
            {
                ArgumentNullException.ThrowIfNull(Collection);

                return Value >= 0 && Value < Collection.Count;
            }
        }
    }
}