using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Zion.Serialization;

namespace Zion
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RGBAColor : IRGBColor, IBinarySerializable<RGBAColor>
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }


        public RGBAColor(byte Value)
        {
            R = Value;
            G = Value;
            B = Value;
            A = byte.MaxValue;
        }
        public RGBAColor(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = byte.MaxValue;
        }
        public RGBAColor(byte R, byte G, byte B, byte A)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }
        public RGBAColor(RGBColor Color)
        {
            this.R = Color.R;
            this.G = Color.G;
            this.B = Color.B;
            this.A = byte.MaxValue;
        }
        public RGBAColor(RGBColor Color, byte A)
        {
            this.R = Color.R;
            this.G = Color.G;
            this.B = Color.B;
            this.A = A;
        }


        public static bool operator ==(RGBAColor A, RGBAColor B)
        {
            return A.R == B.R && A.G == B.G && A.B == B.B && A.A == B.A;
        }
        public static bool operator !=(RGBAColor A, RGBAColor B)
        {
            return A.R != B.R || A.G != B.G || A.B != B.B || A.A != B.A;
        }

        public static bool operator ==(RGBAColor A, RGBColor B)
        {
            return A.R == B.R && A.G == B.G && A.B == B.B;
        }
        public static bool operator !=(RGBAColor A, RGBColor B)
        {
            return A.R != B.R || A.G != B.G || A.B != B.B;
        }

        public static RGBAColor operator *(RGBAColor A, float B)
        {
            return new RGBAColor
            (
                (byte)Math.Clamp(A.R * B, 0f, 255f),
                (byte)Math.Clamp(A.G * B, 0f, 255f),
                (byte)Math.Clamp(A.B * B, 0f, 255f),
                A.A
            );
        }

        public static implicit operator RGBColor(RGBAColor Color)
        {
            return new RGBColor(Color.R, Color.G, Color.B);
        }
        public static implicit operator RGBAColor(RGBColor Color)
        {
            return new RGBAColor(Color.R, Color.G, Color.B);
        }


        public override readonly string ToString()
        {
            return $"({R}, {G}, {B}, {A})";
        }

        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is RGBAColor Color && this == Color;
        }

        public override int GetHashCode()
        {
            return R | (G << 8) | (B << 16) | (A << 24);
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(R);
            Writer.Write(G);
            Writer.Write(B);
            Writer.Write(A);
        }

        public static RGBAColor Read(BinaryReader Reader)
        {
            return new RGBAColor
            (
                Reader.ReadByte(),
                Reader.ReadByte(),
                Reader.ReadByte(),
                Reader.ReadByte()
            );
        }
    }
}