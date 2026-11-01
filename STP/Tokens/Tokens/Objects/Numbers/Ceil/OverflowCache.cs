using System.Numerics;

namespace Zion.STP
{
    internal readonly struct OverflowCache<T> where T : IComparable<T>, INumber<T>, new()
    {
        public readonly T   MinDivision;
        public readonly T   MaxDivision;
        public readonly int MinRemainder;
        public readonly int MaxRemainder;

        public OverflowCache(NumberParsingParameters<T> Parameters, int CalculusSystem)
        {
            MinDivision = Parameters.Divide(Parameters.MinValue, CalculusSystem);
            MaxDivision = Parameters.Divide(Parameters.MaxValue, CalculusSystem);
            MinRemainder = Parameters.Remainder(Parameters.MinValue, CalculusSystem);
            MaxRemainder = Parameters.Remainder(Parameters.MaxValue, CalculusSystem);
        }
    }
}