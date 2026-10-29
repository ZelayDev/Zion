using Zion.Serialization;

namespace Zion
{
    public static class ShortExtensions
    {
        public static IBinarySerializer<short> _Serializer = new BinarySerializer<short>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadInt16()
        );

        extension(short Value)
        {
            public static IBinarySerializer<short> Serializer => _Serializer;


            /// <summary>
            /// Gets the state of a specific bit in the short integer.
            /// </summary>
            /// <param name="Value">The short integer value.</param>
            /// <param name="Index">The bit position (0-15).</param>
            /// <returns>True if the bit is set, otherwise false.</returns>
            public bool GetBit(int Index)
            {
                return (Value & (1 << Index)) != 0;
            }

            /// <summary>
            /// Sets or clears a specific bit in the short integer.
            /// </summary>
            /// <param name="Value">The short integer value.</param>
            /// <param name="Index">The bit position (0-15).</param>
            /// <param name="Bit">True to set the bit, false to clear it.</param>
            /// <returns>The modified short integer value.</returns>
            public short SetBit(int Index, bool Bit)
            {
                return Bit ? (short)(Value | (1 << Index)) : (short)(Value & ~(1 << Index));
            }


            public IEnumerable<bool> EnumerateBits()
            {
                return EnumerateBits(16);
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
            /// Checks if the short integer value is even.
            /// </summary>
            /// <param name="Value">The short integer value.</param>
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

                short MaxValue = (short)Math.Sqrt(Value);

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