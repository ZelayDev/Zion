using Zion.Serialization;

namespace Zion
{
    public static class DoubleExtensions
    {
        public static IBinarySerializer<double> _Serializer = new BinarySerializer<double>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadDouble()
        );

        extension(double Value)
        {
            public static IBinarySerializer<double> Serializer => _Serializer;

            /// <summary>
            /// Rounds the double value to the nearest integer using specified rounding mode.
            /// </summary>
            /// <param name="Value">The value to round.</param>
            /// <param name="Mode">The rounding mode to use.</param>
            /// <returns>The rounded integer value.</returns>
            public int RoundToInt(RoundMode Mode = RoundMode.Round)
            {
                return (int)Value.Round(Mode);
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
            /// Performs linear interpolation between two double values.
            /// </summary>
            /// <param name="A">The starting value.</param>
            /// <param name="B">The ending value.</param>
            /// <param name="Alpha">The interpolation factor (0-1).</param>
            /// <returns>The interpolated value between A and B.</returns>
            public static double Lerp(double A, double B, double Alpha)
            {
                return A + ((B - A) * Alpha);
            }
        }
    }
}