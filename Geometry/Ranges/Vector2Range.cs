using System.Diagnostics.CodeAnalysis;
using Zion.Serialization;

namespace Zion.Geometry
{
    [Serializable]
    public readonly struct Vector2Range : IBinarySerializable<Vector2Range>, IRange<Vector2>, IEqualityComparer<Vector2Range>
    {
        public int BinarySize => 16;

        public Vector2 Start { get; }
        public Vector2 End { get; }

        public Vector2Range(Vector2 Start, Vector2 End)
        {
            ArgumentOutOfRangeException.ThrowIf(!(Start < End), nameof(Start), $"(Start={Start}) can not >= End(={End})");

            this.Start = Start;
            this.End = End;
        }


        public override string ToString()
        {
            return $"[{Start}..{End})";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        public override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is Vector2Range Range && this == Range;
        }


        public static bool operator ==(Vector2Range A, Vector2Range B)
        {
            return A.Start == B.Start && A.End == B.End;
        }

        public static bool operator !=(Vector2Range A, Vector2Range B)
        {
            return A.Start != B.Start || A.End != B.End;
        }


        public bool Equals(Vector2Range A, Vector2Range B)
        {
            return A == B;
        }

        public int GetHashCode(Vector2Range Object)
        {
            return Object.GetHashCode();
        }


        public bool IsInside(Vector2 Value)
        {
            return Value >= Start && Value < End;
        }

        public bool IsInside<R>(R Range) where R : IRange<Vector2>
        {
            return IsInside(Range.Start) && Range.End <= End;
        }

        public bool Overlap<R>(R Range) where R : IRange<Vector2>
        {
            return Range.End > Start && Range.Start < End;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Start);
            Writer.Write(End);
        }

        public static Vector2Range Read(BinaryReader Reader)
        {
            return new Vector2Range
            (
                Reader.Read<Vector2>(),
                Reader.Read<Vector2>()
            );
        }
    }
}