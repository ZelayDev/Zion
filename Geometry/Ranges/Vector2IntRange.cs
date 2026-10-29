using System.Diagnostics.CodeAnalysis;
using Zion.Serialization;

namespace Zion.Geometry
{
    [Serializable]
    public readonly struct Vector2IntRange : IBinarySerializable<Vector2IntRange>, IRange<Vector2Int>, IEqualityComparer<Vector2IntRange>
    {
        public int BinarySize => 16;

        public Vector2Int Start { get; }
        public Vector2Int End { get; }

        public Vector2IntRange(Vector2Int Start, Vector2Int End)
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
            return Object is Vector2IntRange Range && this == Range;
        }


        public static bool operator ==(Vector2IntRange A, Vector2IntRange B)
        {
            return A.Start == B.Start && A.End == B.End;
        }

        public static bool operator !=(Vector2IntRange A, Vector2IntRange B)
        {
            return A.Start != B.Start || A.End != B.End;
        }


        public bool Equals(Vector2IntRange A, Vector2IntRange B)
        {
            return A == B;
        }

        public int GetHashCode(Vector2IntRange Object)
        {
            return Object.GetHashCode();
        }


        public bool IsInside(Vector2Int Value)
        {
            return Value >= Start && Value < End;
        }

        public bool IsInside<R>(R Range) where R : IRange<Vector2Int>
        {
            return IsInside(Range.Start) && Range.End <= End;
        }

        public bool Overlap<R>(R Range) where R : IRange<Vector2Int>
        {
            return Range.End > Start && Range.Start < End;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Start);
            Writer.Write(End);
        }

        public static Vector2IntRange Read(BinaryReader Reader)
        {
            return new Vector2IntRange
            (
                Reader.Read<Vector2Int>(),
                Reader.Read<Vector2Int>()
            );
        }
    }
}