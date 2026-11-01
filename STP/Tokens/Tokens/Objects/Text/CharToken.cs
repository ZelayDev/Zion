namespace Zion.STP
{
    public sealed class CharToken : ValueToken<char>
    {
        public override string ToString()
        {
            return $"[Char:'{Value}']";
        }
    }

    public readonly struct CharTokenReader : ITokenReader
    {
        private static readonly Dictionary<char, char> EscapeChars = new()
        {
            { '\\', '\\' },
            { '"', '\"' },
            { '0', '\0' },
            { 'a', '\a' },
            { 'b', '\b' },
            { 'f', '\f' },
            { 't', '\t' },
            { 'v', '\v' },
            { 'r', '\r' },
            { 'n', '\n' }
        };

        public bool Read(ref TextSource Source, out Token Token)
        {
            if (!Source.CurrentIs('\''))
            {
                Token = default!;
                return false;
            }

            Source.MoveNext();

            Token = ReadCharContent(ref Source);
            return true;
        }

        private Token ReadCharContent(ref TextSource Source)
        {
            if (Source.IsEnd)
            {
                return new CharToken { Length = 1, ErrorCodes = [6010] };
            }

            if (Source.Current == '\\')
            {
                Source.MoveNext();
                return ReadEscapeChar(ref Source);
            }
            return ReadSimpleChar(ref Source);
        }


        private Token ReadSimpleChar(ref TextSource Source)
        {
            var Char = Source.Current;
            Source.MoveNext();

            return Source.CurrentIs('\'')
                ? new CharToken { Length = 3, Value = Char }
                : new CharToken { Length = 2, ErrorCodes = [6013] };
        }

        private Token ReadEscapeChar(ref TextSource Source)
        {
            if (Source.IsEnd)
            {
                return new CharToken { Length = 2, ErrorCodes = [6013] };
            }

            switch (Source.Current)
            {
                case 'u': return ReadHexFormat(ref Source, 4);
                case 'U': return ReadHexFormat(ref Source, 8);
                case 'x': return ReadHexFormat(ref Source);
                default:  return ReadNamedEscape(ref Source);
            }
        }


        private static Token ReadHexFormat(ref TextSource Source, int HexLength)
        {
            Source.MoveNext();

            var Length = 3;
            var HexReaded = 0;
            var HexCode = 0;

            while (!Source.IsEnd && HexReaded < HexLength && Source.Current.IsHexadecimalDigit(out var Digit))
            {
                HexCode = (HexCode << 4) | Digit;
                Source.MoveNext();
                HexReaded++;
                Length++;
            }

            if (HexReaded < HexLength)
            {
                while (!Source.IsEnd && Source.Current != '\'')
                {
                    Length++;
                    Source.MoveNext();
                }

                if (!Source.IsEnd)
                {
                    Source.MoveNext();
                }

                return new CharToken { Length = Length + 1, ErrorCodes = [6011] };
            }

            if (Source.IsEnd || Source.Current != '\'')
            {
                while (!Source.IsEnd && Source.Current != '\'')
                {
                    Source.MoveNext();
                    Length++;
                }
                if (!Source.IsEnd)
                {
                    Source.MoveNext();
                }

                return new CharToken { Length = Length + 1, ErrorCodes = [6013] };
            }

            Source.MoveNext();
            return new CharToken { Length = Length + 1, Value = (char)HexCode };
        }

        private static Token ReadHexFormat(ref TextSource Source)
        {
            Source.MoveNext();

            var Length = 3;
            var HexValue = 0;
            var HexReaded = 0;
            var HasInvalidHex = false;

            while (!Source.IsEnd && HexReaded < 4 && Source.Current != '\'')
            {
                if (Source.Current.IsHexadecimalDigit(out var digit))
                {
                    HexValue = (HexValue << 4) | digit;
                    HexReaded++;
                }
                else
                {
                    HasInvalidHex = true;
                }
                Source.MoveNext();
                Length++;
            }

            if (HexReaded == 0 && !HasInvalidHex)
            {
                if (!Source.CurrentIs('\''))
                {
                    Source.MoveNext();
                }
                return new CharToken { Length = Length + 1, ErrorCodes = [6011] };
            }

            if (!Source.CurrentIs('\''))
            {
                Source.MoveNext();
                return HasInvalidHex
                    ? new CharToken { Length = Length + 1, Value = (char)HexValue, ErrorCodes = [6011] }
                    : new CharToken { Length = Length + 1, Value = (char)HexValue };
            }

            while (!Source.IsEnd && Source.Current != '\'')
            {
                Source.MoveNext();
                Length++;
            }
            if (!Source.IsEnd)
            {
                Source.MoveNext();
            }

            return new CharToken { Length = Length + 1, ErrorCodes = [6013] };
        }

        private static Token ReadNamedEscape(ref TextSource Source)
        {
            if (Source.Current == '\'')
            {
                Source.MoveNext();

                return Source.CurrentIs('\'')
                    ? new CharToken { Length = 4, Value = '\'' }
                    : new CharToken { Length = 3, Value = '\\' };
            }

            if (EscapeChars.TryGetValue(Source.Current, out var EscapeChar))
            {
                Source.MoveNext();

                if (Source.CurrentIs('\''))
                {
                    Source.MoveNext();
                    return new CharToken { Length = 4, Value = EscapeChar };
                }

                Source.MoveNext();
                return new CharToken { Length = 3, ErrorCodes = [6013] };
            }

            var QuoteChecker = Source.BeginNew();

            QuoteChecker.MoveNext();

            if (Source.CurrentIs('\''))
            {
                Source = QuoteChecker;
                return new CharToken { Length = 3, ErrorCodes = [6012] };
            }
            return new CharToken { Length = 2, ErrorCodes = [6013] };
        }
    }
}