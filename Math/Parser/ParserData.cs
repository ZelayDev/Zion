namespace Zion.MathExpressions
{
    public sealed partial class MathExpressionParser
    {
        private readonly Amount Result = new();

        private readonly string String;
        private          char   Current => String[Index];
        private          int    Index;
        private          bool   Finished => Index >= String.Length || Current is '|' or '\\' or ')';

        public int Accuracy
        {
            private get => field;
            init => field = Math.Max(1, value);
        } = 14;

        private static readonly Dictionary<char, char> CharsAssociations = new()
        {
            { '{', '(' },
            { '}', ')' },
            { '[', '(' },
            { ']', ')' },
            { '.', ',' }
        };

        private static readonly ParameterInfo[] Value = [new(typeof(Fraction), "Value")];

        private static readonly ParameterInfo[][] Group =
            [[new ParameterInfo(typeof(Fraction), "Values") { IsEnumerable = true }]];

        public Dictionary<string, MathFunction> Functions { get; init; } = new(StringComparer.OrdinalIgnoreCase)
        {
            //... sin, cos, tan, cot, log  {  ln (_e), log (_10), log(A|B)  }

            {
                "sqrt",
                new MathFunction
                (
                    MathFunctions.Sqrt,
                    [
                        Value,
                        [new ParameterInfo(typeof(Fraction), "Value"), new ParameterInfo(typeof(int), "Degree")],
                        [new ParameterInfo(typeof(Fraction), "Value"), new ParameterInfo(typeof(Fraction), "Degree")]
                    ],
                    "N-th root of a number"
                )
            },
            {
                "abs",
                new MathFunction
                (
                    MathFunctions.Convert(Fraction.Abs),
                    Value,
                    "Abs value of a number"
                )
            },
            {
                "neg",
                new MathFunction
                (
                    MathFunctions.Convert(Fraction.ToNegative),
                    Value,
                    "Negative value of a number"
                )
            },
            {
                "sign",
                new MathFunction
                (
                    MathFunctions.Convert(Fraction.Sign),
                    Value,
                    "Number sign indicator (-1, 0, 1)"
                )
            },
            {
                "round",
                new MathFunction
                (
                    MathFunctions.Convert(Fraction.Round),
                    Value,
                    "Number rounded to the nearest integer"
                )
            },
            {
                "floor",
                new MathFunction
                (
                    MathFunctions.Convert(Fraction.Floor),
                    Value,
                    "Rounding down towards"
                )
            },
            {
                "ceiling",
                new MathFunction
                (
                    MathFunctions.Convert(Fraction.Ceiling),
                    Value,
                    "Rounding up towards"
                )
            },
            {
                "trunc",
                new MathFunction
                (
                    MathFunctions.Convert(Fraction.Truncate),
                    Value,
                    "Integer part of a number"
                )
            },
            {
                "sumr",
                new MathFunction
                (
                    MathFunctions.SumRange,
                    [new ParameterInfo(typeof(Fraction), "Start"), new ParameterInfo(typeof(Fraction), "End")],
                    "Sum of an integer sequence from start to end"
                )
            },
            {
                "min",
                new MathFunction
                (
                    MathFunctions.ConvertGrouped(Fraction.Min),
                    Group,
                    "Smallest value in a set"
                )
            },
            {
                "max",
                new MathFunction
                (
                    MathFunctions.ConvertGrouped(Fraction.Max),
                    Group,
                    "Largest value in a set"
                )
            },
            {
                "sum",
                new MathFunction
                (
                    MathFunctions.ConvertGrouped(Values => Fraction.Sum(Values)),
                    Group,
                    "Total of all values"
                )
            },
            {
                "aver",
                new MathFunction
                (
                    MathFunctions.ConvertGrouped(Values => Fraction.Average(Values)),
                    Group,
                    "Arithmetic mean of values"
                )
            }
        };

        public Dictionary<string, Fraction> Variables { internal get; init; } = new(StringComparer.OrdinalIgnoreCase)
        {
            { "pi", Fraction.Pi },
            { "e", Fraction.E }
        };
    }
}