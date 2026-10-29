using Zion.Serialization;

namespace Zion
{
    public static class UIntExtensions
    {
        public static IBinarySerializer<uint> _Serializer = new BinarySerializer<uint>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadUInt32()
        );

        extension(uint Value)
        {
            public static IBinarySerializer<uint> Serializer => _Serializer;


            /// <summary>
            /// Gets the state of a specific bit in the unsigned integer.
            /// </summary>
            /// <param name="Value">The unsigned integer value.</param>
            /// <param name="Index">The bit position (0-31).</param>
            /// <returns>True if the bit is set, otherwise false.</returns>
            public bool GetBit(int Index)
            {
                return (Value & (1u << Index)) != 0;
            }

            /// <summary>
            /// Sets or clears a specific bit in the unsigned integer.
            /// </summary>
            /// <param name="Value">The unsigned integer value.</param>
            /// <param name="Index">The bit position (0-31).</param>
            /// <param name="Bit">True to set the bit, false to clear it.</param>
            /// <returns>The modified unsigned integer value.</returns>
            public uint SetBit(int Index, bool Bit)
            {
                return Bit ? Value | (1u << Index) : Value & ~(1u << Index);
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
            /// Checks if the unsigned integer value is even.
            /// </summary>
            /// <param name="Value">The unsigned integer value.</param>
            /// <returns>True if the value is even, otherwise false.</returns>
            public bool IsEven()
            {
                return (Value & 1u) == 0;
            }

            public bool IsPrime()
            {
                if (Value <= 1u) { return false; }
                if (Value == 2) { return true; }
                if (IsEven(Value)) { return false; }

                uint MaxValue = (uint)Math.Sqrt(Value);

                for (uint i = 3; i <= MaxValue; i += 2)
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