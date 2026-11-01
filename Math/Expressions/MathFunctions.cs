namespace Zion.MathExpressions
{
    public static class MathFunctions
    {
        public static IMathTerm Sqrt(FunctionHandler Handler)
        {
            var Value = Handler.ReadMathTerm();

            var Degree = 2;
            if (Handler.ReadInt32(out var NewDegree))
            {
                Degree = NewDegree;
            }

            return new SimpleFunction
            (
                Value,
                (Value, Accuracy) => Fraction.Sqrt
                (
                    Value.GetValue(Accuracy),
                    Degree,
                    Accuracy
                )
            );
        }

        public static IMathTerm SumRange(FunctionHandler Handler)
        {
            return new RangeAmount
            (
                Handler.ReadMathTerm(),
                Handler.ReadMathTerm()
            );
        }

        public static Fraction Factorial(IMathTerm Value, int Accuracy)
        {
            return Fraction.Factorial(Value.GetValue(Accuracy));
        }

        public static Fraction DoubleFactorial(IMathTerm Value, int Accuracy)
        {
            return Fraction.DoubleFactorial(Value.GetValue(Accuracy));
        }


        public static MathFunctionHandler Convert(this Func<IMathTerm, Fraction> Function)
        {
            return Handler =>
            {
                var Value = Handler.ReadMathTerm();
                return new SimpleFunction
                (
                    Value,
                    (Value, Accuracy) => Function(Value)
                );
            };
        }

        public static MathFunctionHandler Convert(this Func<IMathTerm, int, Fraction> Function)
        {
            return Handler =>
            {
                var Value = Handler.ReadMathTerm();
                return new SimpleFunction
                (
                    Value,
                    (Value, Accuracy) => Function(Value, Accuracy)
                );
            };
        }

        public static MathFunctionHandler Convert(this Func<Fraction, Fraction> Function)
        {
            return Handler =>
            {
                var Value = Handler.ReadMathTerm();
                return new SimpleFunction
                (
                    Value,
                    (Value, Accuracy) => Function(Value.GetValue(Accuracy))
                );
            };
        }


        public static MathFunctionHandler ConvertGrouped(this Func<IList<IMathTerm>, int, Fraction> Function)
        {
            return Handler =>
            {
                var Values = new List<IMathTerm>();

                while (!Handler.Finished)
                {
                    Values.Add((Fraction)Handler.ReadInt32());
                }

                return new GroupFunction(Values, Function);
            };
        }

        public static MathFunctionHandler ConvertGrouped(this Func<IList<IMathTerm>, Fraction> Function)
        {
            return Handler =>
            {
                var Values = new List<IMathTerm>();

                while (!Handler.Finished)
                {
                    Values.Add(Handler.ReadMathTerm());
                }

                return new GroupFunction(Values, (Values, Accuracy) => Function(Values));
            };
        }


        public static MathFunctionHandler ConvertGrouped(this Func<Fraction[], int, Fraction> Function)
        {
            return Handler =>
            {
                var Values = new List<IMathTerm>();

                while (!Handler.Finished)
                {
                    Values.Add((Fraction)Handler.ReadInt32());
                }

                return new GroupFunction(Values,
                    (Values, Accuracy) => Function(List.Convert(Values, Value => Value.GetValue(Accuracy)), Accuracy));
            };
        }

        public static MathFunctionHandler ConvertGrouped(this Func<Fraction[], Fraction> Function)
        {
            return Handler =>
            {
                var Values = new List<IMathTerm>();

                while (!Handler.Finished)
                {
                    Values.Add(Handler.ReadMathTerm());
                }

                return new GroupFunction(Values,
                    (Values, Accuracy) => Function(List.Convert(Values, Value => Value.GetValue(Accuracy))));
            };
        }
    }
}