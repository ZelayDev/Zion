using System.Collections;
using System.Runtime.InteropServices.Marshalling;
using Zion.Serialization;

namespace Zion
{
    public readonly struct Segment : IEquatable<Segment>, IBinarySerializable<Segment>
    {
        public static readonly Segment Empty = new Segment(0, 0);

        public readonly int Start;
        public readonly int Count;


        public Segment(int Start, int Count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Start);
            ArgumentOutOfRangeException.ThrowIfNegative(Count);
            this.Start = Start;
            this.Count = Count;
        }


        public static bool operator ==(Segment A, Segment B)
        {
            return A.Start == B.Start && A.Count == B.Count;
        }

        public static bool operator !=(Segment A, Segment B)
        {
            return A.Start != B.Start || A.Count != B.Count;
        }


        public override string ToString()
        {
            return $"[Start:{Start}, Count:{Count}]";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, Count);
        }

        public override bool Equals(object? Object)
        {
            return Object is Segment Segment && this == Segment;
        }


        public void Out(out int Start, out int Count)
        {
            Start = this.Start;
            Count = this.Count;
        }

        public bool IsWithin(ICollection Collection)
        {
            return Start + Count < Collection.NotNull().Count;
        }

        public bool IsWithout(ICollection Collection)
        {
            return Start + Count >= Collection.NotNull().Count;
        }


        public void ThrowIfWithout(ICollection Collection)
        {
            if (IsWithout(Collection))
            {
                throw new IndexOutOfRangeException($"Range out of Collection range");
            }
        }


        public bool Equals(Segment Other)
        {
            return this == Other;
        }


        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Start);
            Writer.Write(Count);
        }

        public static Segment Read(BinaryReader Reader)
        {
            return new Segment
            (
                Reader.ReadInt32(),
                Reader.ReadInt32()
            );
        }
    }
}