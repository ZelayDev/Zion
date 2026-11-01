using System.Numerics;

namespace Zion
{
    public static class FloatNumbersExtensions
    {
        extension<T>(T Value) where T : IFloatingPointIeee754<T>
        {
            /// <summary>
            ///     Rounds a floating-point value using specified rounding mode.
            /// </summary>
            /// <typeparam name="T">The floating-point type.</typeparam>
            /// <param name="Value">The value to round.</param>
            /// <param name="Mode">The rounding mode to use.</param>
            /// <returns>The rounded value.</returns>
            public T Round(RoundMode Mode = RoundMode.Round)
            {
                switch (Mode)
                {
                    case RoundMode.Floor:
                        return T.Floor(Value);
                    case RoundMode.Ceiling:
                        return T.Ceiling(Value);
                    default:
                        return T.Round(Value);
                }
            }
        }
    }
}