using Zion.Serialization;

namespace Zion
{
    public static class DecimalExtensions
    {
        public static IBinarySerializer<decimal> _Serializer = new BinarySerializer<decimal>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadDecimal()
        );

        extension(decimal Value)
        {
            public static IBinarySerializer<decimal> Serializer => _Serializer;

            /// <summary>
            /// Rounds the decimal value to the nearest integer using specified rounding mode.
            /// </summary>
            /// <param name="Value">The value to round.</param>
            /// <param name="Mode">The rounding mode to use.</param>
            /// <returns>The rounded integer value.</returns>
            public int RoundToInt(RoundMode Mode = RoundMode.Round)
            {
                return (int)Round(Value, Mode);
            }

            /// <summary>
            /// Rounds the double value to the nearest integer64 using specified rounding mode.
            /// </summary>
            /// <param name="Value">The value to round.</param>
            /// <param name="Mode">The rounding mode to use.</param>
            /// <returns>The rounded integer value.</returns>
            public long RoundToInt64(RoundMode Mode = RoundMode.Round)
            {
                return (long)Value.Round(Mode);
            }

            /// <summary>
            /// Rounds the decimal value using specified rounding mode.
            /// </summary>
            /// <param name="Value">The value to round.</param>
            /// <param name="Mode">The rounding mode to use.</param>
            /// <returns>The rounded value.</returns>
            public decimal Round(RoundMode Mode = RoundMode.Round)
            {
                switch (Mode)
                {
                    case RoundMode.Floor:
                        return Math.Floor(Value);
                    case RoundMode.Ceiling:
                        return Math.Ceiling(Value);
                    default:
                        return Math.Round(Value);
                }
            }

            /// <summary>
            /// Performs linear interpolation between two decimal values.
            /// </summary>
            /// <param name="A">The starting value.</param>
            /// <param name="B">The ending value.</param>
            /// <param name="Alpha">The interpolation factor (0-1).</param>
            /// <returns>The interpolated value between A and B.</returns>
            public static decimal Lerp(decimal A, decimal B, decimal Alpha)
            {
                return A + ((B - A) * Alpha);
            }
        }
    }
}