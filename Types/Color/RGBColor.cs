using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using Zion.Serialization;

namespace Zion
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RGBColor : IBinarySerializable<RGBColor>, IEnumerable<byte>
    {
        public int BinarySize => 3;

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        /// <summary>
        /// Range: [0; 255]
        /// </summary>
        public float Brightness => (R * 0.2126f) + (G * 0.7152f) + (B * 0.0722f);

        public static readonly RGBColor Black = new RGBColor(0);
        public static readonly RGBColor DarkGray = new RGBColor(128);
        public static readonly RGBColor Gray = new RGBColor(182);
        public static readonly RGBColor White = new RGBColor(255);

        public static readonly RGBColor DarkBlue = new RGBColor(0, 0, 255);
        public static readonly RGBColor DarkGreen = new RGBColor(85, 177, 85);
        public static readonly RGBColor DarkCyan = new RGBColor(0, 128, 128);
        public static readonly RGBColor DarkRed = new RGBColor(128, 0, 0);
        public static readonly RGBColor DarkMagenta = new RGBColor(128, 0, 128);
        public static readonly RGBColor DarkYellow = new RGBColor(128, 128, 0);

        public static readonly RGBColor Blue = new RGBColor(41, 161, 221);
        public static readonly RGBColor Green = new RGBColor(0, 255, 0);
        public static readonly RGBColor Cyan = new RGBColor(0, 255, 255);
        public static readonly RGBColor Red = new RGBColor(255, 0, 0);
        public static readonly RGBColor Magenta = new RGBColor(255, 0, 255);
        public static readonly RGBColor Yellow = new RGBColor(255, 255, 0);
        public static readonly RGBColor Orange = new RGBColor(247, 102, 3);
        public static readonly RGBColor Violet = new RGBColor(144, 40, 241);


        public RGBColor(byte Value)
        {
            R = Value;
            G = Value;
            B = Value;
        }
        public RGBColor(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
        public RGBColor(Color Color)
        {
            R = Color.R;
            G = Color.G;
            B = Color.B;
        }


        public static bool operator ==(RGBColor A, RGBColor B)
        {
            return A.R == B.R && A.G == B.G && A.B == B.B;
        }
        public static bool operator !=(RGBColor A, RGBColor B)
        {
            return A.R != B.R || A.G != B.G || A.B != B.B;
        }

        public static RGBColor operator *(RGBColor A, float B)
        {
            return new RGBColor
            (
                (byte)Math.Clamp(A.R * B, 0f, 255f),
                (byte)Math.Clamp(A.G * B, 0f, 255f),
                (byte)Math.Clamp(A.B * B, 0f, 255f));
        }

        public static implicit operator Color(RGBColor Color)
        {
            return System.Drawing.Color.FromArgb(Color.R, Color.G, Color.B);
        }

        public static implicit operator uint(RGBColor Color)
        {
            return (uint)(Color.R << 16 | Color.G << 8 | Color.B);
        }


        public override string ToString()
        {
            return $"({R}, {G}, {B})";
        }

        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is RGBColor Color && this == Color;
        }

        public override int GetHashCode()
        {
            return R | (G << 8) | (B << 16);
        }


        public static RGBColor Parse(string String)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(String);

            if (String.StartsWith('#') && String.Length == 7)
            {
                if (String.Length == 7)
                {
                    byte R = HexToByte(String[1..3], 'R');
                    byte G = HexToByte(String[3..5], 'G');
                    byte B = HexToByte(String[5..7], 'B');

                    return new RGBColor(R, G, B);
                }
            }

            byte[] Result = new byte[3];

            int Start = 0;
            int Index = 0;

            try
            {
                for (int i = 0; i < 3; i++)
                {
                    Index = String.Skip(Index, Char => !char.IsDigit(Char));
                    Start = Index;
                    Index = String.Skip(Index, char.IsDigit);

                    Result[i] = byte.Parse(String[Start..Index]);
                }
            }
            catch (Exception Exception)
            {
                throw new FormatException($"Couldn't convert String(=\"{String}\") to Color", Exception);
            }
            return new RGBColor(Result[0], Result[1], Result[2]);
        }

        public static bool TryParse(string String, out RGBColor Color)
        {
            try
            {
                Color = Parse(String);
                return true;
            }
            catch
            {
                Color = default;
                return false;
            }
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(R);
            Writer.Write(G);
            Writer.Write(B);
        }

        public static RGBColor Read(BinaryReader Reader)
        {
            return new RGBColor
            (
                Reader.ReadByte(),
                Reader.ReadByte(),
                Reader.ReadByte()
            );
        }


        public string ToHexString()
        {
            string Format = "X2";
            return $"#{R.ToString(Format)}{G.ToString(Format)}{B.ToString(Format)}";
        }

        public readonly RGBColor GetOpposite()
        {
            return new RGBColor((byte)(byte.MaxValue - R), (byte)(byte.MaxValue - G), (byte)(byte.MaxValue - B));
        }


        private static byte HexToByte(string String, object Identifier)
        {
            try
            {
                return Convert.ToByte(String, 16);
            }
            catch
            {
                throw new FormatException($"Incorrect hex format(={String}) of ({Identifier})");
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            yield return R;
            yield return G;
            yield return B;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}