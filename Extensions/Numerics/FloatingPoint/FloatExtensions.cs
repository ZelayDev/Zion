using Zion.Serialization;

namespace Zion
{
    public static class FloatExtensions
    {
        public static IBinarySerializer<float> _Serializer = new BinarySerializer<float>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadSingle()
        );

        extension(float Value)
        {
            public static IBinarySerializer<float> Serializer => _Serializer;


            /// <summary>
            /// Rounds the float value to the nearest integer using specified rounding mode.
            /// </summary>
            /// <param name="Value">The value to round.</param>
            /// <param name="Mode">The rounding mode to use.</param>
            /// <returns>The rounded integer value.</returns>
            public int RoundToInt(RoundMode Mode = RoundMode.Round)
            {
                return (int)Value.Round(Mode);
            }

            /// <summary>
            /// Performs linear interpolation between two float values.
            /// </summary>
            /// <param name="A">The starting value.</param>
            /// <param name="B">The ending value.</param>
            /// <param name="Alpha">The interpolation factor (0-1).</param>
            /// <returns>The interpolated value between A and B.</returns>
            public static float Lerp(float A, float B, float Alpha)
            {
                return A + ((B - A) * Alpha);
            }
        }        
    }
}