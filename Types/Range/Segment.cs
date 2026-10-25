using System.Collections;
using System.Runtime.InteropServices;
using Zion.Serialization;

namespace Zion
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Segment : IEquatable<Segment>, IBinarySerializable<Segment>
    {
        public static readonly Segment            Empty            = new(0, 0);
        public static readonly IComparer<Segment> ByLengthComparer = new ByLengthSegmentComparer();
        public static readonly IComparer<Segment> ByStartComparer  = new ByStartSegmentComparer();
        public static readonly IComparer<Segment> ByEndComparer    = new ByEndSegmentComparer();

        public readonly int Start;
        public readonly int Length;

        public int End => Start + Length;


        public Segment(int Start, int Length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Start);
            ArgumentOutOfRangeException.ThrowIfNegative(Length);
            this.Start = Start;
            this.Length = Length;
        }


        public static bool operator ==(Segment A, Segment B)
        {
            return A.Start == B.Start && A.Length == B.Length;
        }

        public static bool operator !=(Segment A, Segment B)
        {
            return A.Start != B.Start || A.Length != B.Length;
        }


        public override string ToString()
        {
            return $"[Start:{Start}, Count:{Length}]";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, Length);
        }

        public override bool Equals(object? Object)
        {
            return Object is Segment Segment && this == Segment;
        }


        public void Out(out int Start, out int Count)
        {
            Start = this.Start;
            Count = Length;
        }

        public bool IsWithin(ICollection Collection)
        {
            return Start + Length < Collection.NotNull().Count;
        }

        public bool IsWithout(ICollection Collection)
        {
            return Start + Length >= Collection.NotNull().Count;
        }


        public void ThrowIfWithout(ICollection Collection)
        {
            if (IsWithout(Collection))
            {
                throw new IndexOutOfRangeException("Range out of Collection range");
            }
        }


        public bool Equals(Segment Other)
        {
            return this == Other;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Start);
            Writer.Write(Length);
        }

        public static Segment Read(BinaryReader Reader)
        {
            return new Segment
            (
                Reader.ReadInt32(),
                Reader.ReadInt32()
            );
        }


        private sealed class ByLengthSegmentComparer : IComparer<Segment>
        {
            public int Compare(Segment A, Segment B)
            {
                return A.Length.CompareTo(B.Length);
            }
        }

        private sealed class ByStartSegmentComparer : IComparer<Segment>
        {
            public int Compare(Segment A, Segment B)
            {
                return A.Start.CompareTo(B.Start);
            }
        }

        private sealed class ByEndSegmentComparer : IComparer<Segment>
        {
            public int Compare(Segment A, Segment B)
            {
                return A.End.CompareTo(B.End);
            }
        }
    }
}