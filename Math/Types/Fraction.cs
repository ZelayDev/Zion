using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using Int = System.Numerics.BigInteger;

namespace Zion.MathExpressions
{
    public struct Fraction : IMathTerm
    {
        public static readonly Fraction NaN = new Fraction(0, 0);
        public static readonly Fraction Zero = new Fraction(0);
        public static readonly Fraction One = new Fraction(1);
        public static readonly Fraction MinusOne = new Fraction(-1);
        public static readonly Fraction Pi = new Fraction(2646693125139304345, 842468587426513207);
        public static readonly Fraction E = new Fraction(325368125, 119696244);

        public Int Divisible { get; private set; }
        public Int Divider { get; private set; }

        public bool IsNaN => Divider == 0;
        public bool IsPositive => !IsNaN && Int.IsPositive(Divisible);
        public bool IsNegative => !IsNaN && Int.IsNegative(Divisible);
        public bool IsZero => !IsNaN && Divisible == 0;
        public bool IsOne => !IsNaN && Divisible == Divider;

        public Int Ceil => Divisible / Divider;
        public Int Remainder => Int.Abs(Divisible % Divider);

        public Fraction Inversed => IsNaN || IsZero ? NaN : new Fraction(Divider, Divisible);


        public Fraction(Int Ceil)
        {
            Divisible = Ceil;
            Divider = 1;
        }
        public Fraction(Int Divisible, Int Divider)
        {
            if (Divider == 0)
            {
                this = NaN;
                return;
            }

            if (Int.IsNegative(Divider))
            {
                this.Divisible = -Divisible;
                this.Divider = -Divider;
            }
            else
            {
                this.Divisible = Divisible;
                this.Divider = Divider;
            }
        }


        public static Fraction operator +(Fraction A, Fraction B)
        {
            if (A.IsNaN || B.IsNaN) return NaN;
            if (A.IsZero) return B;
            if (B.IsZero) return A;

            if (A.Divider == B.Divider)
            {
                return new Fraction(A.Divisible + B.Divisible, A.Divider).SoftSimplify();
            }

            if (A.Divider % B.Divider == 0)
            {
                Int Factor = A.Divider / B.Divider;
                return new Fraction(A.Divisible + (B.Divisible * Factor), A.Divider).SoftSimplify();
            }

            if (B.Divider % A.Divider == 0)
            {
                Int Factor = B.Divider / A.Divider;
                return new Fraction((A.Divisible * Factor) + B.Divisible, B.Divider).SoftSimplify();
            }

            Int NewDivider = A.Divider * B.Divider;
            return new Fraction(
                (A.Divisible * B.Divider) + (B.Divisible * A.Divider),
                NewDivider
            ).SoftSimplify();
        }
        public static Fraction operator -(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN ? NaN : B.IsZero ? A : A + new Fraction(-B.Divisible, B.Divider);
        }

        public static Fraction operator *(Fraction A, Fraction B)
        {
            if (A.IsNaN || B.IsNaN) return NaN;
            if (A.IsZero || B.IsZero) return Zero;
            if (A.IsOne) return B;
            if (B.IsOne) return A;

            Int Gcd1 = GetGreatestCommonDivisor(A.Divisible, B.Divider);
            Int Gcd2 = GetGreatestCommonDivisor(B.Divisible, A.Divider);

            Int NewDivisible = A.Divisible / Gcd1 * (B.Divisible / Gcd2);
            Int NewDivider = A.Divider / Gcd2 * (B.Divider / Gcd1);

            return new Fraction(NewDivisible, NewDivider);
        }
        public static Fraction operator /(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN || B.IsZero
                ? NaN
                : A.IsZero ? Zero : B.IsOne ? A : new Fraction(A.Divisible * B.Divider, A.Divider * B.Divisible);
        }

        public static Fraction operator %(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN || B.IsZero ? NaN : A.IsZero ? Zero : B.IsOne ? Zero : A - (B * Floor(A / B));
        }

        public static Fraction operator +(Fraction A, Int B)
        {
            return A.IsNaN
                ? NaN
                : B == 0 ? A : A.IsZero ? new Fraction(B) : new Fraction(A.Divisible + (A.Divider * B), A.Divider).SoftSimplify();
        }
        public static Fraction operator -(Fraction A, Int B)
        {
            return A.IsNaN ? NaN : B == 0 ? A : A + (-B);
        }

        public static Fraction operator *(Fraction A, Int B)
        {
            if (A.IsNaN) return NaN;
            if (B == 0) return Zero;
            if (B == 1) return A;
            if (A.IsZero) return Zero;
            if (A.IsOne) return new Fraction(B);

            Int Gcd = GetGreatestCommonDivisor(B, A.Divider);
            Int NewDivisible = A.Divisible * (B / Gcd);
            Int NewDivider = A.Divider / Gcd;

            return new Fraction(NewDivisible, NewDivider);
        }
        public static Fraction operator /(Fraction A, Int B)
        {
            if (A.IsNaN || B == 0) return NaN;
            if (B == 1) return A;
            if (A.IsZero) return Zero;

            Int Gcd = GetGreatestCommonDivisor(A.Divisible, B);
            Int NewDivisible = A.Divisible / Gcd;
            Int NewDivider = A.Divider * (B / Gcd);

            return new Fraction(NewDivisible, NewDivider);
        }

        public static Fraction operator <<(Fraction A, int B)
        {
            return A.IsNaN
                ? NaN
                : B == 0
                ? A
                : B < 0
                ? throw new ArgumentOutOfRangeException($"B(={B}) < 0")
                : new Fraction
            (
                A.Divisible << B,
                A.Divider
            );
        }
        public static Fraction operator >>(Fraction A, int B)
        {
            return A.IsNaN
                ? NaN
                : B == 0
                ? A
                : B < 0
                ? throw new ArgumentOutOfRangeException($"B(={B}) < 0")
                : new Fraction
            (
                A.Divisible >> B,
                A.Divider
            );
        }

        public static Fraction operator -(Fraction A)
        {
            return new Fraction(-A.Divisible, A.Divider);
        }

        public static bool operator ==(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN ? false : ReferenceEquals(A, B) ? true : A.Divisible * B.Divider == A.Divider * B.Divisible;
        }
        public static bool operator !=(Fraction A, Fraction B)
        {
            return !(A == B);
        }

        public static bool operator <(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN ? false : A.Divisible * B.Divider < B.Divisible * A.Divider;
        }
        public static bool operator >(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN ? false : A.Divisible * B.Divider > B.Divisible * A.Divider;
        }

        public static bool operator <(Fraction A, Int B)
        {
            return A.IsNaN ? false : A.Divisible < B * A.Divider;
        }
        public static bool operator >(Fraction A, Int B)
        {
            return A.IsNaN ? false : A.Divisible > B * A.Divider;
        }

        public static bool operator <(Int A, Fraction B)
        {
            return B.IsNaN ? false : A * B.Divider < B.Divisible;
        }
        public static bool operator >(Int A, Fraction B)
        {
            return B.IsNaN ? false : A * B.Divider > B.Divisible;
        }

        public static bool operator <=(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN ? false : A.Divisible * B.Divider <= B.Divisible * A.Divider;
        }
        public static bool operator >=(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN ? false : A.Divisible * B.Divider >= B.Divisible * A.Divider;
        }

        public static bool operator <=(Fraction A, Int B)
        {
            return A.IsNaN ? false : A.Divisible <= B * A.Divider;
        }
        public static bool operator >=(Fraction A, Int B)
        {
            return A.IsNaN ? false : A.Divisible >= B * A.Divider;
        }

        public static bool operator <=(Int A, Fraction B)
        {
            return B.IsNaN ? false : A * B.Divider <= B.Divisible;
        }
        public static bool operator >=(Int A, Fraction B)
        {
            return B.IsNaN ? false : A * B.Divider >= B.Divisible;
        }

        public static implicit operator Fraction(Int Value)
        {
            return Value.ToFraction();
        }
        public static implicit operator Fraction(int Value)
        {
            return new Fraction((Int)Value);
        }

        public static explicit operator Int(Fraction Value)
        {
            return Value.Ceil;
        }
        public static explicit operator double(Fraction Value)
        {
            return (double)Value.Divisible / (double)Value.Divider;
        }


        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is Fraction Fraction && this == Fraction;
        }

        public override int GetHashCode()
        {
            if (IsNaN) return 0;

            unchecked
            {
                int Hash1 = Divisible.GetHashCode();
                int Hash2 = Divider.GetHashCode();
                return Hash1 ^ (Hash2 * 397);
            }
        }

        public override string ToString()
        {
            return IsNaN ? "NaN" : IsZero ? "0" : IsOne ? "1" : IsCeil(out Int Value) ? Value.ToString() : $"({Divisible}/{Divider})";
        }

        public string ToDecimalString(int Accuracy, Comma Separator = Comma.Dot)
        {
            if (IsNaN) { return "NaN"; }
            if (IsZero) { return "0"; }
            if (IsOne) { return "1"; }

            if (IsCeil(out Int Ceil))
            {
                return Ceil.ToString();
            }

            Int Divider = this.Divider;
            Int CeilPart = this.Ceil;
            Int Remainder = this.Remainder;

            string CeilPartString = (IsNegative && CeilPart == 0) ? "-0" : CeilPart.ToString();

            StringBuilder Builder = new StringBuilder();
            Int CurrentRemainder = Remainder;

            for (int i = 0; i < Accuracy; i++)
            {
                if (CurrentRemainder == 0)
                {
                    break;
                }

                CurrentRemainder *= 10;
                Int Digit = CurrentRemainder / Divider;
                CurrentRemainder %= Divider;

                Builder.Append(Digit.ToString());
            }

            string String = Builder.ToString();

            return !string.IsNullOrEmpty(String) ? $"{CeilPartString}{(char)Separator}{String}" : CeilPartString;
        }

        public string ToStackedString(bool UseUnicodeChars = false)
        {
            if (IsNaN) { return "NaN"; }
            if (IsZero) { return "0"; }
            if (IsOne) { return "1"; }
            if (IsCeil(out Int Ceil))
            {
                return Ceil.ToString();
            }

            Simplify();

            (Int Ceil, Fraction Fractional) Regular = ToRegular();

            string CeilString = Regular.Ceil.ToString();
            string DivisibleString = Regular.Fractional.Divisible.ToString();
            string DividerString = Regular.Fractional.Divider.ToString();

            if (UseUnicodeChars)
            {
                DivisibleString = Text.ConvertAll(DivisibleString, SpecialChars.ToDegreeDigit);
                DividerString = Text.ConvertAll(DividerString, SpecialChars.ToIndexDigit);
            }

            return CeilString == "0"
                ? $"{DivisibleString}/{DividerString}"
                : $"{CeilString}_{DivisibleString}/{DividerString}";
        }


        public static Fraction Parse(string String)
        {
            String = Normalize(String.NotNull());

            if (String.Length == 0)
            {
                return Zero;
            }

            int DivisionSign = String.IndexOf('/');

            return DivisionSign == -1
                ? ToBigInteger(String).ToFraction()
                : new Fraction
            (
                Int.Parse(String[..DivisionSign]),
                Int.Parse(String[(DivisionSign + 1)..])
            );
        }

        public static bool TryParse(string String, out Fraction Value)
        {
            try
            {
                Value = Parse(String.NotNull());
                return true;
            }
            catch
            {
                Value = default;
                return false;
            }
        }


        public static Fraction ParseDecimal(string String)
        {
            String = NormalizeFloated(String.NotNull());

            bool IsNegative = String.StartsWith('-');
            bool HasSign = IsNegative || String.StartsWith('+');
            int Start = HasSign ? 1 : 0;

            if (String.Length >= Start + 2 && String[Start] == '0' && (String[Start + 1] == 'x' || String[Start + 1] == 'b' || String[Start + 1] == 'X' || String[Start + 1] == 'B'))
            {
                if (String.Length == Start + 2)
                {
                    throw new FormatException("Invalid number format");
                }
                Int Result = ToBigInteger(String);
                return Result.ToFraction();
            }

            int Dot = String.IndexOf(',');

            if (Dot == -1)
            {
                return Int.TryParse(String, out Int Result)
                    ? Result.ToFraction()
                    : throw new FormatException($"Couldn't convert \"{String}\" to Int");
            }

            if (Dot == Start && String.Length > Start + 1)
            {
                string FractionalPartString = String.Substring(Start + 1);
                foreach (char Char in FractionalPartString)
                {
                    if (!char.IsDigit(Char))
                    {
                        throw new FormatException($"Invalid character '{Char}' in fractional part");
                    }
                }

                if (string.IsNullOrEmpty(FractionalPartString))
                {
                    return Zero;
                }

                if (!Int.TryParse(FractionalPartString, out Int Divisible))
                {
                    throw new FormatException($"Couldn't convert fractional part \"{FractionalPartString}\" to Int");
                }

                Fraction Result = new Fraction(Divisible, Int.Pow(10, FractionalPartString.Length));
                return IsNegative ? -Result : Result;
            }

            string IntegerPart = String[Start..Dot];
            string FractionalPart = String[(Dot + 1)..];

            foreach (char Char in FractionalPart)
            {
                if (!char.IsDigit(Char))
                {
                    throw new FormatException($"Invalid character '{Char}' in fractional part");
                }
            }

            if (!Int.TryParse(IntegerPart, out Int IntegerValue))
            {
                throw new FormatException($"Couldn't convert integer part \"{IntegerPart}\" to Int");
            }

            if (string.IsNullOrEmpty(FractionalPart))
            {
                Fraction Result = IntegerValue.ToFraction();
                return IsNegative ? -Result : Result;
            }

            string FullNumberString = IntegerPart + FractionalPart;
            if (!Int.TryParse(FullNumberString, out Int FullNumber))
            {
                throw new FormatException($"Couldn't convert \"{FullNumberString}\" to Int");
            }

            Int Divider = Int.Pow(10, FractionalPart.Length);

            if (IntegerValue < 0)
            {
                FullNumber = -Int.Abs(FullNumber);
            }

            Fraction FinalResult = new Fraction(FullNumber, Divider);
            return IsNegative ? -FinalResult : FinalResult;
        }

        public static bool TryParseDecimal(string String, out Fraction Value)
        {
            String = NormalizeFloated(String.NotNull());
            Value = default;

            bool IsNegative = String.StartsWith('-');
            bool HasSign = IsNegative || String.StartsWith('+');
            int Start = HasSign ? 1 : 0;

            if (String.Length >= Start + 2 && String[Start] == '0' && (String[Start + 1] == 'x' || String[Start + 1] == 'b' || String[Start + 1] == 'X' || String[Start + 1] == 'B'))
            {
                if (String.Length == Start + 2)
                {
                    return false;
                }

                try
                {
                    Int Result = ToBigInteger(String);
                    Value = Result.ToFraction();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            int Dot = String.IndexOf(',');

            if (Dot == -1)
            {
                if (Int.TryParse(String, out Int Result))
                {
                    Value = Result.ToFraction();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (Dot == Start && String.Length > Start + 1)
            {
                string FractionalPartString = String.Substring(Start + 1);
                foreach (char Char in FractionalPartString)
                {
                    if (!char.IsDigit(Char))
                    {
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(FractionalPartString))
                {
                    Value = Zero;
                    return true;
                }

                if (!Int.TryParse(FractionalPartString, out Int Divisible))
                {
                    return false;
                }

                Value = new Fraction(Divisible, Int.Pow(10, FractionalPartString.Length));

                if (IsNegative)
                {
                    Value = -Value;
                }

                return true;
            }

            string IntegerPart = String[Start..Dot];
            string FractionalPart = String[(Dot + 1)..];

            foreach (char Char in FractionalPart)
            {
                if (!char.IsDigit(Char))
                {
                    return false;
                }
            }

            if (!Int.TryParse(IntegerPart, out Int IntegerValue))
            {
                return false;
            }

            if (string.IsNullOrEmpty(FractionalPart))
            {
                Value = IntegerValue.ToFraction();

                if (IsNegative)
                {
                    Value = -Value;
                }

                return true;
            }

            string FullNumberString = IntegerPart + FractionalPart;
            if (!Int.TryParse(FullNumberString, out Int FullNumber))
            {
                return false;
            }

            Int Divider = Int.Pow(10, FractionalPart.Length);

            if (IntegerValue < 0)
            {
                FullNumber = -Int.Abs(FullNumber);
            }

            Value = new Fraction(FullNumber, Divider);

            if (IsNegative)
            {
                Value = -Value;
            }

            return true;
        }


        public Fraction GetValue(int Accuracy)
        {
            return this;
        }


        public Fraction SoftSimplify()
        {
            return IsNaN ? NaN : IsOne ? One : IsZero ? Zero : IsCeil(out Int Value) ? Value.ToFraction() : this;
        }

        public (Int, Fraction) ToRegular()
        {
            return IsNaN ? (0, NaN) : (Ceil, new Fraction(Int.Abs(Remainder), Divider));
        }

        public void Simplify()
        {
            if (IsNaN) { this = NaN; }
            if (IsOne) { this = One; }
            if (IsZero) { this = Zero; }
            if (IsCeil(out Int Value))
            {
                this = Value.ToFraction();
                return;
            }
            if (Divisible == 0)
            {
                Divider = 1;
                return;
            }

            Int Gcd = GetGreatestCommonDivisor(Int.Abs(Divisible), Int.Abs(Divider));

            Divisible /= Gcd;
            Divider /= Gcd;

            if (Divider < 0)
            {
                Divisible = -Divisible;
                Divider = -Divider;
            }
        }

        public bool IsCeil()
        {
            return Divider == 1 || Divisible % Divider == 0;
        }

        public bool IsCeil(out Int Value)
        {
            if (Divider == 1)
            {
                Value = Divisible;
                return true;
            }
            if (Divisible % Divider == 0)
            {
                Value = Divisible / Divider;
                return true;
            }
            Value = default;
            return false;
        }

        public Fraction Reverse()
        {
            return new Fraction(Divider, Divisible);
        }


        public static Fraction GetSimple(Fraction Value)
        {
            Value.Simplify();
            return Value;
        }

        public static Fraction Abs(Fraction Value)
        {
            return Value.IsNaN ? NaN : Value.IsPositive ? Value : new Fraction(-Value.Divisible, Value.Divider);
        }

        public static Fraction Sign(Fraction Value)
        {
            return Value.IsNaN ? NaN : Value.IsZero ? Zero : Value.IsPositive ? One : MinusOne;
        }

        public static Fraction ToNegative(Fraction Value)
        {
            return Value.IsNaN ? NaN : Value.IsNegative ? Value : new Fraction(-Value.Divisible, Value.Divider);
        }

        public static Fraction Round(Fraction Value)
        {
            return Value.IsNaN ? NaN : (Fraction)RoundToInt(Value);
        }

        public static Fraction Ceiling(Fraction Value)
        {
            return Value.IsNaN ? NaN : (Fraction)CeilingToInt(Value);
        }

        public static Fraction Floor(Fraction Value)
        {
            return Value.IsNaN ? NaN : (Fraction)FloorToInt(Value);
        }

        public static Fraction Truncate(Fraction Value)
        {
            return Value.Ceil;
        }

        public static Fraction Max(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN ? NaN : A > B ? A : B;
        }

        public static Fraction Min(Fraction A, Fraction B)
        {
            return A.IsNaN || B.IsNaN ? NaN : A < B ? A : B;
        }


        public static Fraction SumRange(Fraction Min, Fraction Max)
        {
            if (Min.IsNaN || Max.IsNaN) { return NaN; }
            if (Min > Max)
            {
                object.Swap(ref Min, ref Max);
            }

            Int A = CeilingToInt(Min);
            Int B = FloorToInt(Max);

            return Min >= Max ? Zero : (Fraction)(((B - A + Int.One) * (A + B)) >> 1);
        }


        public static Fraction Sum(params IList<Fraction> Values)
        {
            ArgumentNullException.ThrowIfNull(Values);

            if (Values.IsNullOrEmpty()) { return Zero; }
            if (Values.Count == 1) { return Values[0]; }
            if (Values.Any(m => m.IsNaN)) { return NaN; }

            Int CommonDivider = Values[0].Divider;

            for (int i = 1; i < Values.Count; i++)
            {
                CommonDivider = GetLeastCommonMultiple(CommonDivider, Values[i].Divider);
            }

            Int SumDivisibles = 0;
            foreach (Fraction Member in Values)
            {
                Int Factor = CommonDivider / Member.Divider;
                SumDivisibles += Member.Divisible * Factor;
            }

            return new Fraction(SumDivisibles, CommonDivider).SoftSimplify();
        }

        public static Fraction Max(params IEnumerable<Fraction> Values)
        {
            ArgumentNullException.ThrowIfNull(Values);

            Fraction Result = NaN;

            foreach (Fraction Value in Values)
            {
                if ((Result.IsNaN || Value > Result) && !Value.IsNaN)
                {
                    Result = Value;
                }
            }

            return Result;
        }

        public static Fraction Min(params IEnumerable<Fraction> Values)
        {
            ArgumentNullException.ThrowIfNull(Values);

            Fraction Result = NaN;

            foreach (Fraction Value in Values)
            {
                if ((Result.IsNaN || Value < Result) && !Value.IsNaN)
                {
                    Result = Value;
                }
            }

            return Result;
        }

        public static Fraction Average(params IList<Fraction> Values)
        {
            return Sum(Values) / Values.Count;
        }


        public static Fraction Product(params IEnumerable<Fraction> Members)
        {
            ArgumentNullException.ThrowIfNull(Members);

            if (Members.IsNullOrEmpty()) { return Zero; }
            if (!Members.HasAtLeast(2)) { return Members.First(); }
            if (Members.Any(m => m.IsNaN)) { return NaN; }

            Int Divisible = Int.One;
            Int Divider = Int.One;

            foreach (Fraction Member in Members)
            {
                Divisible *= Member.Divisible;
                Divider *= Member.Divider;
            }

            return new Fraction(Divisible, Divider);
        }

        public static Fraction Factorial(Fraction Value)
        {
            if (Value.IsNaN) { return NaN; }
            if (Value.IsZero || Value.IsOne) { return One; }
            if (Value.Divisible <= 0) { return NaN; }

            Int Number = (Int)Value;
            Int Result = Int.One;

            if (Number <= 20)
            {
                for (int i = 2; i <= Number; i++)
                {
                    Result *= i;
                }
                return Result.ToFraction();
            }

            return ParallelFactorial(Number);
        }

        public static Fraction DoubleFactorial(Fraction Value)
        {
            if (Value.IsNaN) { return NaN; }
            if (Value.IsZero || Value.IsOne) { return One; }
            if (Value.Divisible <= 0) { return NaN; }

            Int Number = (Int)Value;
            Int Result = Int.One;

            if (Number <= 20)
            {
                int Start = Number.IsEven ? 2 : 1;

                for (int i = Start; i <= Number; i += 2)
                {
                    Result *= i;
                }

                return Result.ToFraction();
            }

            return ParallelDoubleFactorial(Number);
        }


        public static Fraction Pow(Fraction Value, Fraction Degree, int Accuracy = 14)
        {
            if (Value.IsNaN || Degree.IsNaN) { return NaN; }
            if (Value.IsOne) { return One; }
            if (Degree.IsCeil(out Int CeilDegree)) { return Pow(Value, CeilDegree); }
            if (Accuracy <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Accuracy), $"Accuracy(={Accuracy}) <= 0");
            }

            bool NegativePower = Degree.Divisible < 0;
            Int AbsDivisible = Int.Abs(Degree.Divisible);

            Fraction DivisiblePower = Pow(Value.Divisible.ToFraction(), (int)AbsDivisible);
            Fraction DividerPower = Pow(Value.Divider.ToFraction(), (int)AbsDivisible);

            Fraction Result = Sqrt(DivisiblePower, (int)Degree.Divider, Accuracy) / Sqrt(DividerPower, (int)Degree.Divider, Accuracy);
            return NegativePower ? One / Result : Result;
        }

        public static Fraction Pow(Fraction Value, Int Degree)
        {
            if (Value.IsNaN) { return NaN; }
            if (Degree.IsZero) { return One; }
            if (Int.IsNegative(Degree)) { return Pow(Value, -Degree).Reverse(); }

            Fraction Result = One;
            Fraction Multiplier = Value;

            foreach (bool Bit in EnumerateBits(Degree))
            {
                if (Bit)
                {
                    Result *= Multiplier;
                }

                Multiplier *= Multiplier;
            }

            return Result;
        }

        public static Fraction Sqrt(Fraction Value, int RootDegree, int Accuracy = 14)
        {
            if (Value.IsNaN || RootDegree <= 0) { return NaN; }
            if (RootDegree == 1) { return Value; }
            if (Value.IsZero) { return Zero; }
            if (Value.IsOne) { return One; }

            if (Value < Zero && RootDegree.IsEven())
            {
                return NaN;
            }

            Fraction ExactRoot = TryFindExactRoot(Value, RootDegree);
            if (!ExactRoot.IsNaN)
            {
                return ExactRoot;
            }

            Fraction CurrentApproximation = GetImprovedInitialGuess(Value, RootDegree);
            Fraction PreviousApproximation = CurrentApproximation;
            int StagnationCount = 0;

            for (int Iteration = 0; Iteration < Accuracy; Iteration++)
            {
                Fraction PowerOfApproximation = Pow(CurrentApproximation, RootDegree - 1);
                if (PowerOfApproximation.IsZero)
                {
                    break;
                }

                Fraction NextApproximation =
                    ((new Fraction(RootDegree - 1)
                    * CurrentApproximation) + (Value / PowerOfApproximation))
                    / new Fraction(RootDegree);

                if (NextApproximation == PreviousApproximation)
                {
                    StagnationCount++;
                    if (StagnationCount > 5)
                    {
                        NextApproximation.SoftSimplify();
                        return NextApproximation;
                    }
                }
                else
                {
                    StagnationCount = 0;
                }

                if (Abs(NextApproximation - CurrentApproximation) < new Fraction(1, 1000000))
                {
                    NextApproximation.SoftSimplify();
                    return NextApproximation;
                }

                PreviousApproximation = CurrentApproximation;
                CurrentApproximation = NextApproximation;
            }

            CurrentApproximation.SoftSimplify();
            return CurrentApproximation;
        }

        public static Fraction Sqrt(Fraction Value, Fraction RootDegree, int Accuracy = 14)
        {
            return RootDegree.IsNaN || Value.IsNaN
                ? NaN
                : Value < Zero && RootDegree.IsCeil(out Int CeilDegree) && CeilDegree.IsEven
                ? NaN
                : RootDegree.IsCeil(out Int Degree) && Degree <= int.MaxValue && Degree >= int.MinValue
                ? Sqrt(Value, (int)Degree, Accuracy)
                : Pow(Value, RootDegree.Inversed, Accuracy);
        }


        private static Fraction GetImprovedInitialGuess(Fraction Value, int RootDegree)
        {
            if (RootDegree >= 5)
            {
                if (Value > One)
                {
                    return new Fraction(2);
                }
                else if (Value < One && Value > Zero)
                {
                    return new Fraction(1, 2);
                }
            }

            return Value > One ? One : Value < One && Value > Zero ? Value : Value < Zero ? new Fraction(-1) : One;
        }

        private static Fraction TryFindExactRoot(Fraction Value, int RootDegree)
        {
            Value.Simplify();

            if (Value == Zero)
            {
                return Zero;
            }
            if (Value == One)
            {
                return One;
            }

            if (RootDegree == 2)
            {
                Fraction DivisibleRoot = TryIntegerRoot(Value.Divisible, 2);
                Fraction DividerRoot = TryIntegerRoot(Value.Divider, 2);

                if (!DivisibleRoot.IsNaN && !DividerRoot.IsNaN)
                {
                    return DivisibleRoot / DividerRoot;
                }
            }

            if (RootDegree == 3)
            {
                Fraction DivisibleRoot = TryIntegerRoot(Value.Divisible, 3);
                Fraction DividerRoot = TryIntegerRoot(Value.Divider, 3);

                if (!DivisibleRoot.IsNaN && !DividerRoot.IsNaN)
                {
                    return DivisibleRoot / DividerRoot;
                }
            }

            return TrySmallRoots(Value, RootDegree);
        }

        private static Fraction TryIntegerRoot(Int Number, int RootDegree)
        {
            if (Number == 0)
            {
                return Zero;
            }
            if (Number == 1)
            {
                return One;
            }

            Int CurrentValue = (Number / 2) + 1;
            for (int Iteration = 0; Iteration < 100; Iteration++)
            {
                Int PowerOfCurrentValue = Int.Pow(CurrentValue, RootDegree - 1);
                if (PowerOfCurrentValue == 0)
                {
                    break;
                }

                Int NextValue = (((RootDegree - 1) * CurrentValue) + (Number / PowerOfCurrentValue)) / RootDegree;

                if (NextValue >= CurrentValue)
                {
                    break;
                }
                CurrentValue = NextValue;
            }

            return Int.Pow(CurrentValue, RootDegree) == Number ? CurrentValue.ToFraction() : NaN;
        }

        private static Fraction TrySmallRoots(Fraction Value, int RootDegree)
        {
            for (Int Divisible = 1; Divisible <= 100; Divisible++)
            {
                for (Int Divider = 1; Divider <= 100; Divider++)
                {
                    Fraction TestFraction = new Fraction(Divisible, Divider);
                    Fraction PowerOfTestFraction = Pow(TestFraction, RootDegree);

                    if (PowerOfTestFraction == Value)
                    {
                        return TestFraction;
                    }

                    if (Value.IsNegative)
                    {
                        Fraction NegativeTestFraction = new Fraction(-Divisible, Divider);
                        Fraction PowerOfNegativeTestFraction = Pow(NegativeTestFraction, RootDegree);

                        if (PowerOfNegativeTestFraction == Value)
                        {
                            return NegativeTestFraction;
                        }
                    }
                }
            }

            return NaN;
        }


        private static Fraction ParallelFactorial(Int Value)
        {
            int ProcssorCount = Environment.ProcessorCount;
            Int ChunkSize = Value / ProcssorCount;
            Int[] Members = new Int[ProcssorCount];

            Parallel.For(0, ProcssorCount, i =>
            {
                Int Start = (i * ChunkSize) + 1;
                Int End = (i == ProcssorCount - 1) ? Value : (i + 1) * ChunkSize;

                Int LocalResult = 1;
                for (Int j = Start; j <= End; j++)
                {
                    LocalResult *= j;
                }

                Members[i] = LocalResult;
            });

            Int Result = 1;
            foreach (Int Member in Members)
            {
                Result *= Member;
            }

            return Result.ToFraction();
        }

        private static Fraction ParallelDoubleFactorial(Int Value)
        {
            int ProcessorCount = Environment.ProcessorCount;
            Int Even = Value.IsEven ? 2 : 1;
            Int Count = ((Value - Even) / 2) + 1;

            Int ChunkSize = Count / ProcessorCount;
            Int[] Members = new Int[ProcessorCount];

            Parallel.For(0, ProcessorCount, i =>
            {
                Int Start = i * ChunkSize;
                Int End = (i == ProcessorCount - 1) ? Count - 1 : ((i + 1) * ChunkSize) - 1;

                Int Member = 1;
                for (Int j = Start; j <= End; j++)
                {
                    Int value = Even + (j * 2);
                    Member *= value;
                }
                Members[i] = Member;
            });

            Int Result = 1;
            foreach (Int Member in Members)
            {
                Result *= Member;
            }
            return Result.ToFraction();
        }


        private static Int GetGreatestCommonDivisor(Int A, Int B)
        {
            A = Int.Abs(A);
            B = Int.Abs(B);

            if (A == 0) return B;
            if (B == 0) return A;

            int Shift = 0;

            while (((A | B) & 1) == 0)
            {
                A >>= 1;
                B >>= 1;
                Shift++;
            }

            while ((A & 1) == 0)
            {
                A >>= 1;
            }

            do
            {
                while ((B & 1) == 0)
                {
                    B >>= 1;
                }

                if (A > B)
                {
                    Int Temp = A;
                    A = B;
                    B = Temp;
                }

                B -= A;
            }
            while (B != 0);

            return A << Shift;
        }

        private static Int GetLeastCommonMultiple(Int A, Int B)
        {
            return A == 0 || B == 0 ? (Int)0 : Int.Abs(A * B) / GetGreatestCommonDivisor(A, B);
        }


        private static Int RoundToInt(Fraction Value)
        {
            if (Value.IsCeil(out Int Number))
            {
                return Number;
            }

            Int Ceil = Value.Ceil;
            Fraction Remainder = Value.Remainder;

            return Abs(Remainder) >= (Value.Divider / 2).ToFraction()
                ? Ceil + (Value.Divisible.Sign >= 0 ? 1 : -1)
                : Ceil;
        }

        private static Int CeilingToInt(Fraction Value)
        {
            if (Value.IsCeil(out Int Number)) { return Number; }

            Int Ceil = Value.Ceil;

            return Value.Divisible.Sign >= 0 && !Value.Remainder.IsZero
                ? Ceil + 1
                : Ceil;
        }

        private static Int FloorToInt(Fraction Value)
        {
            if (Value.IsCeil(out Int Number)) { return Number; }

            Int Ceil = Value.Ceil;

            return Value.Divisible.Sign < 0 && !Value.Remainder.IsZero
                ? Ceil - 1
                : Ceil;
        }


        private static IEnumerable<bool> EnumerateBits(BigInteger positiveValue)
        {
            byte[] Bytes = positiveValue.ToByteArray();

            int LastByteIndex = Bytes.Length - 1;

            if (LastByteIndex > 0 && Bytes[LastByteIndex] == 0)
            {
                LastByteIndex--;
            }

            for (int ByteIndex = 0; ByteIndex <= LastByteIndex; ByteIndex++)
            {
                byte Byte = Bytes[ByteIndex];

                int BitsToProcess = (ByteIndex == LastByteIndex)
                    ? GetLastByteBitLength(Byte)
                    : 8;

                for (int BitIndex = 0; BitIndex < BitsToProcess; BitIndex++)
                {
                    yield return ((Byte >> BitIndex) & 1) != 0;
                }
            }
        }

        private static int GetLastByteBitLength(byte LastByte)
        {
            if (LastByte == 0) { return 0; }

            int BitLength = 8;
            while ((LastByte & 128) == 0)
            {
                BitLength--;
                LastByte <<= 1;
            }
            return BitLength;
        }


        private static Int ToBigInteger(string String)
        {
            String = Normalize(String);

            bool IsNegative = String.StartsWith('-');
            int Start = 0;

            if (IsNegative || String.StartsWith('+'))
            {
                Start = 1;
            }

            BigInteger GetResult(BigInteger Value)
            {
                return IsNegative ? -Value : Value;
            }

            if (String.Begins(Start, "0b", true))
            {
                if (String.Length == Start + 2)
                {
                    return BigInteger.Zero;
                }

                BigInteger Result = 0;

                for (int i = Start + 2; i < String.Length; i++)
                {
                    Result <<= 1;
                    char Current = String[i];

                    if (Current == '1')
                    {
                        Result |= 1;
                    }
                    else if (Current != '0')
                    {
                        throw new FormatException($"Couldn't convert \"{String}\" to BigInteger");
                    }
                }
                return GetResult(Result);
            }

            if (String.Begins(Start, "0x", true))
            {
                if (String.Length == Start + 2)
                {
                    return BigInteger.Zero;
                }

                BigInteger Result = 0;

                for (int i = Start + 2; i < String.Length; i++)
                {
                    Result <<= 4;
                    Result |= ToHex(String[i]);
                }
                return GetResult(Result);
            }

            return BigInteger.Parse(String);
        }

        private static int ToHex(char Char)
        {
            return char.IsDigit(Char)
                ? Char - '0'
                : Char >= 'a' && Char <= 'f'
                ? Char - 'a' + 10
                : Char >= 'A' && Char <= 'F' ? Char - 'A' + 10 : throw new FormatException($"Couldn't convert '{Char}' to hex");
        }

        private static string Normalize(string String)
        {
            return String.RemoveChars
            (
                Char => char.IsWhiteSpace(Char) || Char == '_'
            );
        }

        private static string NormalizeFloated(string String)
        {
            return Normalize(String).Replace('.', ',');
        }
    }
}