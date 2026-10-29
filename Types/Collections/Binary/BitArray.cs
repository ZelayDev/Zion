using Zion.Serialization;

namespace Zion
{
    public sealed class BitArray : BitCollection, IBinarySerializable<BitArray>, IParsable<BitArray>
    {
        #region Data
        public static readonly BitArray Empty = new BitArray(0);

        public override int Count { get; }

        #endregion

        #region Constructors
        public BitArray(int Count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Count);

            this.Data = new ulong[GetGroupCount(Count)];
            this.Count = Count;
        }

        public BitArray(IEnumerable<ulong> Data, int Count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Count);

            var GroupCount = GetGroupCount(Count);
            var Array = Data.ToArray();

            if (Array.Length < GroupCount)
            {
                System.Array.Resize(ref Array, GroupCount);
            }

            this.Data = Array;
            this.Count = Count;
        }

        private BitArray(int Length, ulong[] Data)
        {
            this.Count = Length;
            this.Data = Data;
        }

        #endregion

        #region Operators
        public static BitArray operator &(BitArray A, BitArray B)
        {
            return ConvertBits(A, B, static (A, B) => (byte)(A & B));
        }

        public static BitArray operator |(BitArray A, BitArray B)
        {
            return ConvertBits(A, B, static (A, B) => (byte)(A | B));
        }

        public static BitArray operator ^(BitArray A, BitArray B)
        {
            return ConvertBits(A, B, static (A, B) => (byte)(A ^ B));

        }

        public static BitArray operator ~(BitArray Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

            var GroupCount = Value.Data.Length;
            var Result = new ulong[GroupCount];

            Span<ulong> Span = Value.AsSpan();

            for (int i = 0; i < GroupCount; i++)
            {
                Result[i] = ~Span[i];
            }

            return new BitArray(GroupCount, Result);
        }

        #endregion

        #region StaticMethods
        public static BitArray Resize(BitArray? Source, int NewLength)
        {
            if (Source is null)
            {
                return new BitArray(NewLength);
            }

            if (NewLength <= 0)
            {
                Source = Empty;
            }

            ulong[] Result = new ulong[GetGroupCount(NewLength)];

            Array.Copy(Source.Data, Result, Math.Min(Source.Count, NewLength));

            return new BitArray(NewLength, Result);
        }

        #endregion

        #region PublicMethods
        public BitArray Clone()
        {
            return new BitArray(Data, Count);
        }

        public BitList ToBitList()
        {
            return new BitList(Data, Count);
        }

        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Count);
            Writer.BaseStream.Write(AsByteSpan());
        }

        public static BitArray Read(BinaryReader Reader)
        {
            int Count = Reader.ReadInt32();
            int GroupCount = GetGroupCount(Count);
            ulong[] Buffer = new ulong[GroupCount];

            for (int i = 0; i < GroupCount; i++)
            {
                Buffer[i] = Reader.ReadUInt64();
            }

            return new BitArray(Count, Buffer);
        }

        #endregion

        #region IParsable
        public static BitArray Parse(string String, IFormatProvider? Provider)
        {
            var Pair = ParseBitCollection(String);
            return new BitArray(Pair.Item1, Pair.Item2);
        }

        public static bool TryParse(string? String, IFormatProvider? Provider, out BitArray Result)
        {
            if (TryParseBitCollection(String, out var Data, out var Count))
            {
                Result = new BitArray(Data, Count);
                return true;
            }
            Result = null!;
            return false;
        }

        #endregion

        #region PrivateMethods

        private static BitArray ConvertBits(BitArray A, BitArray B, Func<ulong, ulong, ulong> Convert)
        {
            ArgumentNullException.ThrowIfNull(A);
            ArgumentNullException.ThrowIfNull(B);

            int MinLength = Math.Min(A.Data.Length, B.Data.Length);
            int MaxLength = Math.Max(A.Data.Length, B.Data.Length);

            ulong[] Result = new ulong[MaxLength];
            
            Span<ulong> ASpan = A.AsSpan();
            Span<ulong> BSpan = B.AsSpan();

            for (int i = 0; i < MinLength; i++)
            {
                Result[i] = Convert(ASpan[i], BSpan[i]);
            }

            return new BitArray(MaxLength, Result);
        }

        #endregion
    }
}