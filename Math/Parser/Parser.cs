namespace Zion.MathExpressions
{
    public sealed partial class MathExpressionParser
    {
        public IMathTerm Parse()
        {
            var Expression = ParseAdditive();

            if (!Finished)
            {
                Throw(Index, $"Unexpected character '{Current}'");
            }

            return Expression;
        }

        internal IMathTerm ParseBeforeSeparator(out int End)
        {
            var Result = Parse();
            End = Index;
            return Result;
        }


        private IMathTerm ParseAdditive()
        {
            var Left = ParseMultiplicative();

            while (!Finished && Current is '+' or '-')
            {
                var Operator = Current;
                Index++;

                var Right = ParseMultiplicative();

                Left = new Amount(Left, Operator == '+' ? Right : new Negative(Right));
            }

            return Left;
        }

        private IMathTerm ParseMultiplicative()
        {
            var Left = ParseExponential();

            while (!Finished && Current is '*' or '/' or '%' or '(')
            {
                var Operator = Current;

                if (Operator == '(')
                {
                    Left = new Product(Left, ParseBrackets());
                    continue;
                }
                Index++;

                var FloorDivision = false;

                if (Operator == '/' && Current == '/')
                {
                    FloorDivision = true;
                    Index++;
                }

                var Right = ParseExponential();

                if (FloorDivision)
                {
                    return new FloorDivision(Left, Right);
                }
                Left = Operator == '*' ? new Product(Left, Right) :
                    Operator == '/' ? new Division(Left, Right) : new Remainder(Left, Right);
            }

            return Left;
        }

        private IMathTerm ParseExponential()
        {
            var Left = ParseFactorial();

            while (!Finished && Current == '^')
            {
                Index++;
                var Right = ParseExponential();
                Left = new Exponent(Left, Right);
            }

            return Left;
        }

        private IMathTerm ParseFactorial()
        {
            var Term = ParsePrimary();

            while (!Finished && Current == '!')
            {
                Index++;
                if (!Finished && Current == '!')
                {
                    Index++;
                    Term = new SimpleFunction(Term, MathFunctions.DoubleFactorial);
                }
                else
                {
                    Term = new SimpleFunction(Term, MathFunctions.Factorial);
                }
            }

            return Term;
        }

        private IMathTerm ParsePrimary()
        {
            if (Finished)
            {
                return Unfinished();
            }

            var IsNegative = false;
            if (Current == '-')
            {
                IsNegative = true;
                Index++;
            }
            else if (Current == '+')
            {
                Index++;
            }

            IMathTerm Term;

            if (IsDigit(Current))
            {
                Term = ParseNumber();
            }
            else if (Current == '(')
            {
                Term = ParseBrackets();
            }
            else if (IsLetter(Current))
            {
                Term = ParseIdentifier();
            }
            else
            {
                return InvalidChar();
            }

            if (IsNegative)
            {
                Term = new Negative(Term);
            }

            return Term;
        }

        private IMathTerm ParseNumber()
        {
            var Start = Index;
            Index = String.Skip(Index, IsDigit);

            if (Index == Start)
            {
                InvalidChar();
            }

            var NumberString = String[Start..Index];
            return Fraction.ParseDecimal(NumberString);
        }

        private IMathTerm ParseBrackets()
        {
            if (Current != '(')
            {
                return Throw("Expected '('");
            }

            Index++;

            var Value = ParseAdditive();

            if (Index >= String.Length || Current != ')')
            {
                return Throw("Expected ')'");
            }

            Index++;
            return Value;
        }

        private IMathTerm ParseIdentifier()
        {
            var Start = Index;
            Index = String.Skip(Index, IsLetter);
            var Identifier = String[Start..Index];

            return !Finished && Current == '('
                ? ExistsFunction(Identifier)
                    ? ParseFunctionCall(Identifier)
                    : ExistsVariable(Identifier)
                        ? new Product(Variables[Identifier], ParseBrackets())
                        : Throw($"Unknown identifier '{Identifier}'")
                : Variables.TryGetValue(Identifier, out var value)
                    ? value
                    : Throw($"Unknown identifier '{Identifier}'");
        }

        private IMathTerm ParseFunctionCall(string Identifier)
        {
            var Handler = GetHandler(Index + 1);
            var Result = Functions[Identifier].Handler(Handler);
            Index = Handler.Index - 1;

            if (Index >= String.Length || Current != ')')
            {
                Throw($"No closing bracket ')' in function \"{Identifier}\"");
            }

            Index++;
            return Result;
        }
    }
}