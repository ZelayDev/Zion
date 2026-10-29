using System.Diagnostics.CodeAnalysis;
using Zion.Serialization;

namespace Zion.Geometry
{
    [Serializable]
    public readonly struct Vector3Range : IBinarySerializable<Vector3Range>, IRange<Vector3>, IEqualityComparer<Vector3Range>
    {
        public Vector3 Start { get; }
        public Vector3 End { get; }

        public Vector3Range(Vector3 Start, Vector3 End)
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
            return Object is Vector3Range Range && this == Range;
        }


        public static bool operator ==(Vector3Range A, Vector3Range B)
        {
            return A.Start == B.Start && A.End == B.End;
        }

        public static bool operator !=(Vector3Range A, Vector3Range B)
        {
            return A.Start != B.Start || A.End != B.End;
        }


        public bool Equals(Vector3Range A, Vector3Range B)
        {
            return A == B;
        }

        public int GetHashCode(Vector3Range Object)
        {
            return Object.GetHashCode();
        }


        public bool IsInside(Vector3 Value)
        {
            return Value >= Start && Value < End;
        }

        public bool IsInside<R>(R Range) where R : IRange<Vector3>
        {
            return IsInside(Range.Start) && Range.End <= End;
        }

        public bool Overlap<R>(R Range) where R : IRange<Vector3>
        {
            return Range.End > Start && Range.Start < End;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Start);
            Writer.Write(End);
        }

        public static Vector3Range Read(BinaryReader Reader)
        {
            return new Vector3Range
            (
                Reader.Read<Vector3>(),
                Reader.Read<Vector3>()
            );
        }
    }
}