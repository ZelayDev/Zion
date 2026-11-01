using System.Text;

namespace Zion
{
    [Serializable]
    public struct ColorText : IColorText
    {
        public static readonly ColorText Empty = new(String.Empty, default);

        public string   Text;
        public RGBColor Color;

        public int Length => Text.Length;

        public ColorText(object? Text, RGBColor Color)
        {
            this.Text = (Text ?? String.Empty).ToString() ?? String.Empty;
            this.Color = Color;
        }

        public ColorChar this[int Index] => new(Text[Index], Color);

        public override string ToString()
        {
            return $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\u001b[0m";
        }

        public string ToString(out int VisualLength)
        {
            VisualLength = Length;
            return $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\u001b[0m";
        }

        public static string GetText(ICollection<char> Chars, RGBColor Color)
        {
            ArgumentNullException.ThrowIfNull(Chars);

            static int GetLength(byte Value)
            {
                return Value switch
                {
                    >= 100 => 3,
                    >= 10  => 2,
                    _      => 1
                };
            }

            return String.Create(Chars.Count + 14 + Color.Sum(GetLength), Chars, (Span, List) =>
            {
                var g = $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Chars}\u001b[0m";
                var Index = 0;

                Span[Index++] = '\u001b';
                Span[Index++] = '[';
                Span[Index++] = '3';
                Span[Index++] = '8';
                Span[Index++] = ';';
                Span[Index++] = '2';

                foreach (var Color in Color)
                {
                    Span[Index++] = ';';
                    foreach (var Char in Color.ToString())
                    {
                        Span[Index++] = Char;
                    }
                }

                Span[Index++] = 'm';

                foreach (var Char in List)
                {
                    Span[Index++] = Char;
                }

                Span[Index++] = '\u001b';
                Span[Index++] = '[';
                Span[Index++] = '0';
                Span[Index++] = 'm';
            });
        }

        public string ToUnicode()
        {
            return $"\\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\\u001b[0m";
        }

        public static string ToUnicode(string Text, RGBColor Color)
        {
            return $"\\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\\u001b[0m";
        }

        public string ToColorString()
        {
            return ToString();
        }

        public static string GetText(string Text, RGBColor Color)
        {
            return $"\u001b[38;2;{Color.R};{Color.G};{Color.B}m{Text}\u001b[0m";
        }

        public static string GetText(string Text, byte R, byte G, byte B)
        {
            return $"\u001b[38;2;{R};{G};{B}m{Text}\u001b[0m";
        }

        public static string Clear(string String)
        {
            var Start = 0;
            var Index = 0;
            var Builder = new StringBuilder(String.Length);

            while (Index < String.Length)
            {
                if (String.Begins(Index, "\u001b[38;2;"))
                {
                    Index += 7;
                    Index = String.Skip(Index, Char => Char != ';') + 1;
                    Index = String.Skip(Index, Char => Char != ';') + 1;
                    Index = String.Skip(Index, Char => Char != 'm') + 1;

                    Start = Index;

                    Index = String.Skip(Index, Char => Char != '\u001b');

                    Builder.Append(String[Start..Index]);

                    if (Index < String.Length && String[Index] == '\u001b')
                    {
                        Index = String.Skip(Index, Char => Char != 'm') + 1;
                    }
                }
                else if (String[Index] != '\u001b')
                {
                    Builder.Append(String[Index]);
                    Index++;
                }
                else
                {
                    Index++;
                }
            }
            return Builder.ToString();
        }

        public static bool IsColored(string String)
        {
            return String.Contains("\u001b[38;2;");
        }

        public static string ConcatToString(params ColorText[] Array)
        {
            return String.Concat(Array);
        }

        public static MassColorText Concat(out int VisualLength, params ColorText[] Array)
        {
            var Result = new MassColorText(Array);
            VisualLength = Result.Length;
            return Result;
        }

        public static string operator +(ColorText A, ColorText B)
        {
            return A + B;
        }

        public static string operator +(ColorText A, ColorChar B)
        {
            return A + B;
        }

        public static string operator +(ColorChar A, ColorText B)
        {
            return A + B;
        }
    }
}