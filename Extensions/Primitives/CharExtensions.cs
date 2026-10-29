using Zion.Serialization;

namespace Zion
{
    public static class CharExtensions
    {
        public static IBinarySerializer<char> _Serializer = new BinarySerializer<char>
        (
            static (Writer, Value) => Writer.Write(Value),
            static Reader => Reader.ReadChar()
        );

        extension(char Char)
        {
            public static IBinarySerializer<char> Serializer => _Serializer;

            public bool IsBinaryDigit()
            {
                return Char == '0' || Char == '1';
            }

            public bool IsOctalDigit()
            {
                return Char >= '0' && Char <= '7';
            }

            public bool IsDigit()
            {
                return Char >= '0' && Char <= '9';
            }

            public bool IsHexadecimalDigit()
            {
                return (Char >= '0' && Char <= '9')
                    || (Char >= 'a' && Char <= 'f')
                    || (Char >= 'A' && Char <= 'F');
            }


            public bool IsBinaryDigit(out int Digit)
            {
                if (Char == '0')
                {
                    Digit = 0;
                    return true;
                }
                if (Char == '1')
                {
                    Digit = 1;
                    return true;
                }
                Digit = default;
                return false;
            }

            public bool IsOctalDigit(out int Digit)
            {
                if (Char >= '0' && Char <= '7')
                {
                    Digit = Char - '0';
                    return true;
                }
                Digit = default;
                return false;
            }

            public bool IsDigit(out int Digit)
            {
                if (Char >= '0' && Char <= '9')
                {
                    Digit = Char - '0';
                    return true;
                }
                Digit = default;
                return false;
            }

            public bool IsHexadecimalDigit(out int Digit)
            {
                if (Char >= '0' && Char <= '9')
                {
                    Digit = Char - '0';
                    return true;
                }
                if (Char >= 'a' && Char <= 'f')
                {
                    Digit = Char - 'a' + 10;
                    return true;
                }
                if (Char >= 'A' && Char <= 'F')
                {
                    Digit = Char - 'A' + 10;
                    return true;
                }
                Digit = default;
                return false;
            }
        }
    }
}