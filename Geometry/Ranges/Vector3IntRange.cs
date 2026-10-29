using System.Diagnostics.CodeAnalysis;
using Zion.Serialization;

namespace Zion.Geometry
{
    [Serializable]
    public readonly struct Vector3IntRange : IBinarySerializable<Vector3IntRange>, IRange<Vector3Int>, IEqualityComparer<Vector3IntRange>
    {
        public int BinarySize => 24;

        public Vector3Int Start { get; }
        public Vector3Int End { get; }

        public Vector3IntRange(Vector3Int Start, Vector3Int End)
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
            return Object is Vector3IntRange Range && this == Range;
        }


        public static bool operator ==(Vector3IntRange A, Vector3IntRange B)
        {
            return A.Start == B.Start && A.End == B.End;
        }

        public static bool operator !=(Vector3IntRange A, Vector3IntRange B)
        {
            return A.Start != B.Start || A.End != B.End;
        }


        public bool Equals(Vector3IntRange A, Vector3IntRange B)
        {
            return A == B;
        }

        public int GetHashCode(Vector3IntRange Object)
        {
            return Object.GetHashCode();
        }


        public bool IsInside(Vector3Int Value)
        {
            return Value >= Start && Value < End;
        }

        public bool IsInside<R>(R Range) where R : IRange<Vector3Int>
        {
            return IsInside(Range.Start) && Range.End <= End;
        }

        public bool Overlap<R>(R Range) where R : IRange<Vector3Int>
        {
            return Range.End > Start && Range.Start < End;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Start);
            Writer.Write(End);
        }

        public static Vector3IntRange Read(BinaryReader Reader)
        {
            return new Vector3IntRange
            (
                Reader.Read<Vector3Int>(),
                Reader.Read<Vector3Int>()
            );
        }
    }
}