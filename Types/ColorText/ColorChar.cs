using Zion.Serialization;

namespace Zion
{
    [Serializable]
    public struct ColorChar : IColorText, IBinarySerializable<ColorChar>
    {
        public char     Char;
        public RGBColor Color;

        public ColorChar(char Char)
        {
            this.Char = Char;
            Color = RGBColor.White;
        }

        public ColorChar(char Char, RGBColor Color)
        {
            this.Char = Char;
            this.Color = Color;
        }

        public override string ToString()
        {
            return $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Char}\u001b[0m";
        }

        public string ToColorString()
        {
            return ToString();
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Char);
            Writer.Write<RGBColor>(Color);
        }

        public static ColorChar Read(BinaryReader Reader)
        {
            return new ColorChar
            (
                Reader.ReadChar(),
                Reader.Read<RGBColor>()
            );
        }
    }
}