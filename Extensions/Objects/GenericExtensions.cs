using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Zion
{
    public static class GenericExtensions
    {
        extension<T>(T Value)
        {
            [return: NotNull]
            public T NotNull([CallerArgumentExpression(nameof(Value))] string? ParameterName = null)
            {
                ArgumentNullException.ThrowIfNull(Value, ParameterName);
                return Value;
            }

            public T Out(out T Out)
            {
                Out = Value;
                return Value;
            }
        }

        /// <summary>
        ///     Checks if the value is within the specified range (inclusive).
        ///     Value >= Min && Value <= Max
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="Value">The value to check.</param>
        /// <param name="Min">The minimum bound of the range.</param>
        /// <param name="Max">The maximum bound of the range.</param>
        /// <returns>True if the value is within bounds, otherwise false.</returns>
        public static bool IsBetween<T>(this T Value, T Min, T Max) where T : IComparable<T>
        {
            return Value.CompareTo(Min) >= 0 && Value.CompareTo(Max) <= 0;
        }

        /// <summary>
        ///     Checks if the value is within the specified range (not inclusive).
        ///     Value >= Min && Value < Max
        /// </summary>
        /// <typeparam name="T">The type of value to compare.</typeparam>
        /// <param name="Value">The value to check.</param>
        /// <param name="Min">The minimum bound of the range.</param>
        /// <param name="Max">The maximum bound of the range.</param>
        /// <returns>True if the value is within bounds, otherwise false.</returns>
        public static bool IsInRange<T>(this T Value, T Min, T Max) where T : IComparable<T>
        {
            return Value.CompareTo(Min) >= 0 && Value.CompareTo(Max) < 0;
        }
    }
}