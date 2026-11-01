using System.Numerics;

namespace Zion.STP
{
    public sealed class IntegerTokenReader<T, I> : ITokenReader where T : ValueToken<I>, new()
        where I : IComparable<I>, INumber<I>, new()
    {
        public readonly NumberParsingParameters<I> NumberParameters;

        private readonly Lazy<OverflowCache<I>> BinaryCache;
        private readonly Lazy<OverflowCache<I>> OctalCache;
        private readonly Lazy<OverflowCache<I>> DecimalCache;
        private readonly Lazy<OverflowCache<I>> HexadecimalCache;

        public IDigitParser DigitParser
        {
            get;
            init => field = value.NotNull();
        } = STP.DigitParser.Instance;

        public bool IgnoreSuffixes { get; init; }

        public IntegerTokenReader(NumberParsingParameters<I> Parameters)
        {
            NumberParameters = Parameters;

            if (Parameters.HasLimits)
            {
                BinaryCache = new Lazy<OverflowCache<I>>(() => new OverflowCache<I>(Parameters, 2));
                OctalCache = new Lazy<OverflowCache<I>>(() => new OverflowCache<I>(Parameters, 8));
                DecimalCache = new Lazy<OverflowCache<I>>(() => new OverflowCache<I>(Parameters, 10));
                HexadecimalCache = new Lazy<OverflowCache<I>>(() => new OverflowCache<I>(Parameters, 16));
            }
        }


        public bool Read(ref TextSource Source, out Token Token)
        {
            if (Source.IsEnd)
            {
                Token = default!;
                return false;
            }

            var IsNegative = false;
            var CharCount = 0;

            if (Source.Current == '-')
            {
                CharCount++;

                if (NumberParameters.OnlyPositive)
                {
                    Source.MoveNext();

                    if (!Source.IsEnd && IsZero(ref Source, out Token, CharCount))
                    {
                        return true;
                    }

                    Token = default!;
                    return false;
                }

                Source.MoveNext();
                IsNegative = true;
                CharCount++;
            }

            bool CheckSuffix(ref TextSource Source, out Token Token)
            {
                var SuffixLength = 0;

                if (BeginsSuffix(ref Source, ref SuffixLength))
                {
                    Token = new T { Length = CharCount + SuffixLength, ErrorCodes = [6000] };
                    return true;
                }

                Token = default!;
                return false;
            }

            if (Source.Begins(['0', DigitParser.BinaryPrefix], out Source))
            {
                if (CheckSuffix(ref Source, out Token))
                {
                    return true;
                }
                return ReadBinary(ref Source, out Token, IsNegative, CharCount + 2);
            }
            if (Source.Begins(['0', DigitParser.OctalPrefix], out Source))
            {
                if (CheckSuffix(ref Source, out Token))
                {
                    return true;
                }
                return ReadOctal(ref Source, out Token, IsNegative, CharCount + 2);
            }
            if (Source.Begins(['0', DigitParser.HexadecimalPrefix], out Source))
            {
                if (CheckSuffix(ref Source, out Token))
                {
                    return true;
                }
                return ReadHexadecimal(ref Source, out Token, IsNegative, CharCount + 2);
            }

            if (CheckSuffix(ref Source, out Token))
            {
                return true;
            }
            return ReadDecimal(ref Source, out Token, IsNegative, CharCount);
        }


        private bool ReadBinary(ref TextSource Source, out Token Token, bool IsNegative, int CharCount)
        {
            return Read(ref Source, out Token, IsNegative, CharCount, DigitParser.IsBinary, BinaryCache,
                Value => NumberParameters.LeftShift(Value, 1), true);
        }

        private bool ReadOctal(ref TextSource Source, out Token Token, bool IsNegative, int CharCount)
        {
            return Read(ref Source, out Token, IsNegative, CharCount, DigitParser.IsOctal, OctalCache,
                Value => NumberParameters.LeftShift(Value, 3), true);
        }

        private bool ReadHexadecimal(ref TextSource Source, out Token Token, bool IsNegative, int CharCount)
        {
            return Read(ref Source, out Token, IsNegative, CharCount, DigitParser.IsHexadecimal, HexadecimalCache,
                Value => NumberParameters.LeftShift(Value, 4));
        }

        private bool ReadDecimal(ref TextSource Source, out Token Token, bool IsNegative, int CharCount)
        {
            return Read(ref Source, out Token, IsNegative, CharCount, DigitParser.IsDecimal, DecimalCache,
                Value => NumberParameters.Multiply(Value, 10));
        }


        private bool Read(ref TextSource Source,     out Token              Token,
            bool                         IsNegative, int                    CharCount,
            SafeConverter<char, int>     IsDigit,    Lazy<OverflowCache<I>> OverflowCache,
            Func<I, I>                   Shift,
            bool                         ForgivingNumbers = false)
        {
            var StartCharCount = CharCount;

            SkipZeros(Source, ref CharCount);
            InitializeVariables(out Token, out var Value, out var Parameters);

            while (!Source.IsEnd)
            {
                if (IsSeparator(Source, ref CharCount))
                {
                    continue;
                }

                if (BeginsSuffix(ref Source, ref CharCount))
                {
                    break;
                }

                if (IsDigit(Source.Current, out var Digit))
                {
                    if (IsNegative)
                    {
                        Digit = -Digit;
                    }

                    if (CheckOverflow(Value, Digit, OverflowCache))
                    {
                        Token = ReadErrorToken(ref Source, CharCount,
                            ForgivingNumbers ? static Char => Char.IsDigit() : IsDigit.ToFunc(), 6003);
                        return true;
                    }

                    Value = Shift(Value);

                    if (Digit != 0)
                    {
                        Value = Parameters.Sum(Value, Digit);
                    }
                }
                else if (ForgivingNumbers && Source.Current.IsDigit())
                {
                    Token = ReadErrorToken(ref Source, CharCount, Char => Char.IsDigit(), 6002);
                    return true;
                }
                else
                {
                    return false;
                }

                Source.MoveNext();
                CharCount++;
            }

            if (CharCount == StartCharCount)
            {
                Token = new T { Length = CharCount, ErrorCodes = [6001] };
                return true;
            }

            Token = CreateToken(Value, CharCount);
            return true;
        }

        private Token ReadErrorToken(ref TextSource Source, int CharCount, Func<char, bool> IsDigit, short ErrorCode)
        {
            var Parameters = NumberParameters;

            while (!Source.IsEnd)
            {
                var Current = Source.Current;

                Source.Skip(Char => Char == '_' || IsDigit(Char), out var Skipped);
                CharCount += Skipped;

                BeginsSuffix(ref Source, ref CharCount);

                break;
            }

            return new T { Value = default!, Length = CharCount, ErrorCodes = [ErrorCode] };
        }


        private bool CheckOverflow(I Value, int Digit, Lazy<OverflowCache<I>> LazyCache)
        {
            var Parameters = NumberParameters;

            if (!Parameters.HasLimits)
            {
                return false;
            }

            var Cache = LazyCache.Value;

            var Zero = I.Zero;

            if (Value.CompareTo(Zero) > 0)
            {
                var MaxDivision = Cache.MaxDivision;

                if (Value.CompareTo(MaxDivision) > 0)
                {
                    return true;
                }

                if (Value.CompareTo(MaxDivision) == 0)
                {
                    var MaxRemainder = Cache.MaxRemainder;
                    return Digit > MaxRemainder;
                }
            }
            else
            {
                var MinDivision = Cache.MinDivision;

                if (Value.CompareTo(MinDivision) < 0)
                {
                    return true;
                }

                if (Value.CompareTo(MinDivision) == 0)
                {
                    var MinRemainder = Cache.MinRemainder;
                    return Digit < MinRemainder;
                }
            }

            return false;
        }

        private bool BeginsSuffix(ref TextSource Source, ref int CharCount)
        {
            var Parameters = NumberParameters;

            if (!IgnoreSuffixes && Source.Current.IsEnglish())
            {
                foreach (var Suffix in Parameters.Suffixes)
                {
                    if (Source.Begins(Suffix, out Source))
                    {
                        CharCount += Suffix.Length;
                        return true;
                    }
                }
            }

            return false;
        }

        private void InitializeVariables(out Token Token, out I Value, out NumberParsingParameters<I> Parameters)
        {
            Token = default!;
            Value = new I();
            Parameters = NumberParameters;
        }

        private bool IsZero(ref TextSource Source, out Token Token, int CharCount)
        {
            var StartCharCount = CharCount;

            SafeConverter<char, int> IsDigit;

            bool IsEnd(ref TextSource Source)
            {
                return !Source.CurrentIs(Char => IsDigit(Char, out _)) || BeginsSuffix(ref Source, ref CharCount);
            }

            if (Source.Begins("0b", out Source) || Source.Begins("0o", out Source))
            {
                CharCount += 2;
                IsDigit = DigitParser.IsDecimal;

                if (IsEnd(ref Source))
                {
                    Token = new T { Length = CharCount, ErrorCodes = [6001] };
                    return true;
                }
            }
            else
            {
                if (Source.Begins("0x", out Source))
                {
                    CharCount += 2;
                    IsDigit = DigitParser.IsHexadecimal;
                }
                else
                {
                    IsDigit = DigitParser.IsDecimal;
                }

                if (IsEnd(ref Source))
                {
                    Token = new T { Length = CharCount, ErrorCodes = [6001] };
                    return true;
                }
            }

            while (!Source.IsEnd && IsDigit(Source.Current, out var Digit))
            {
                if (Digit != 0)
                {
                    Token = ReadErrorToken(ref Source, CharCount, IsDigit.ToFunc(), 6002);
                    return true;
                }

                CharCount++;
                Source.MoveNext();
            }

            Token = CreateToken(new I(), CharCount);
            return true;
        }


        private static bool IsSeparator(TextSource Source, ref int CharCount)
        {
            if (Source.Current == '_')
            {
                Source.MoveNext();
                CharCount++;
                return true;
            }
            return false;
        }

        private static void SkipZeros(TextSource Source, ref int CharCount)
        {
            Source.Skip('0', out var ZerosReaded);
            CharCount += ZerosReaded;
        }

        private static T CreateToken(I Value, int Length)
        {
            return new T { Value = Value, Length = Length };
        }
    }
}