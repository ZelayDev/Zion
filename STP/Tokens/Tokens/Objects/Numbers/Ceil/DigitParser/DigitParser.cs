namespace Zion.STP
{
    public sealed class DigitParser : IDigitParser
    {
        public static readonly DigitParser Instance = new();


        public char BinaryPrefix => 'b';

        public char OctalPrefix => 'o';

        public char HexadecimalPrefix => 'x';


        public bool IsBinary(char Char, out int Digit)
        {
            return Char.IsBinaryDigit(out Digit);
        }

        public bool IsOctal(char Char, out int Digit)
        {
            return Char.IsOctalDigit(out Digit);
        }

        public bool IsDecimal(char Char, out int Digit)
        {
            return Char.IsDigit(out Digit);
        }

        public bool IsHexadecimal(char Char, out int Digit)
        {
            return Char.IsHexadecimalDigit(out Digit);
        }
    }
}