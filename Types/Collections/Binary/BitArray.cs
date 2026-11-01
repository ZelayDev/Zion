using Zion.Serialization;

namespace Zion
{
    public sealed class BitArray : BitCollection, IBinarySerializable<BitArray>, IParsable<BitArray>
    {
        #region Data
        public static readonly BitArray Empty = new(0);

        public override int Length { get; }
        #endregion

        #region Constructors
        public BitArray(int Capacity) : base(Capacity)
        {
            Length = Capacity;
        }

        public BitArray(IEnumerable<ulong> Data, int Capacity) : base(0)
        {
            ArgumentNullException.ThrowIfNull(Data);
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);

            Length = Capacity;

            var GroupCount = GetGroupCount(Capacity);
            var Buffer = Data.ToArray();

            if (Buffer.Length != GroupCount)
            {
                System.Array.Resize(ref Buffer, GroupCount);
            }

            MaskRemainder(Buffer, Capacity);

            Bits = Buffer;
        }

        private BitArray(int Length, ulong[] Bits, bool TakeOwnership) : base(0)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Length);
            ArgumentNullException.ThrowIfNull(Bits);

            var GroupCount = GetGroupCount(Length);

            if (Bits.Length < GroupCount)
            {
                throw new ArgumentException
                (
                    "Bits length is smaller than the required group count for the given length.",
                    nameof(Bits)
                );
            }

            this.Length = Length;

            if (TakeOwnership && Bits.Length == GroupCount)
            {
                this.Bits = Bits;
            }
            else
            {
                var Copy = new ulong[GroupCount];
                System.Array.Copy(Bits, Copy, GroupCount);
                MaskRemainder(Copy, Length);
                this.Bits = Copy;
            }

            if (TakeOwnership)
            {
                MaskRemainder(this.Bits, Length);
            }
        }
        #endregion

        #region Operators
        public static BitArray operator &(BitArray Left, BitArray Right)
        {
            return ConvertBits(Left, Right, static (A, B) => A & B);
        }

        public static BitArray operator |(BitArray Left, BitArray Right)
        {
            return ConvertBits(Left, Right, static (A, B) => A | B);
        }

        public static BitArray operator ^(BitArray Left, BitArray Right)
        {
            return ConvertBits(Left, Right, static (A, B) => A ^ B);
        }

        public static BitArray operator ~(BitArray Value)
        {
            ArgumentNullException.ThrowIfNull(Value);

            var GroupCount = Value.Bits.Length;
            var Result = new ulong[GroupCount];

            for (var Group = 0; Group < GroupCount; Group++)
            {
                Result[Group] = ~Value.Bits[Group];
            }

            return new BitArray(Value.Length, Result, TakeOwnership: true);
        }
        #endregion

        #region StaticMethods
        public static void Resize(ref BitArray? BitArray, int NewLength)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(NewLength);

            if (BitArray is null)
            {
                BitArray = new BitArray(NewLength);
                return;
            }

            if (NewLength == 0)
            {
                BitArray = Empty;
                return;
            }

            var GroupCount = GetGroupCount(NewLength);
            var Result = new ulong[GroupCount];

            var CopyGroupCount = Math.Min(BitArray.Bits.Length, GroupCount);
            System.Array.Copy(BitArray.Bits, Result, CopyGroupCount);

            MaskRemainder(Result, NewLength);

            BitArray = new BitArray(NewLength, Result, TakeOwnership: true);
        }
        #endregion

        #region PublicMethods
        public BitArray Clone()
        {
            var Copy = new ulong[Bits.Length];
            System.Array.Copy(Bits, Copy, Bits.Length);
            return new BitArray(Length, Copy, TakeOwnership: true);
        }

        public BitList ToBitList()
        {
            var Copy = new ulong[Bits.Length];
            System.Array.Copy(Bits, Copy, Bits.Length);
            return new BitList(Copy, Length);
        }
        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer)
        {
            ArgumentNullException.ThrowIfNull(Writer);

            Writer.Write(Length);
            Writer.BaseStream.Write(AsByteSpan().Slice(0, GetByteCount()));
        }

        public static BitArray Read(BinaryReader Reader)
        {
            ArgumentNullException.ThrowIfNull(Reader);

            var Count = Reader.ReadInt32();
            ArgumentOutOfRangeException.ThrowIfNegative(Count);

            var GroupCount = GetGroupCount(Count);
            var Buffer = new ulong[GroupCount];

            for (var Group = 0; Group < GroupCount; Group++)
            {
                Buffer[Group] = Reader.ReadUInt64();
            }

            return new BitArray(Count, Buffer, TakeOwnership: true);
        }
        #endregion

        #region IParsable
        public static BitArray Parse(string String, IFormatProvider? Provider)
        {
            var (Data, Count) = ParseBitCollection(String);
            return new BitArray(Data, Count);
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
        private static BitArray ConvertBits(BitArray Left, BitArray Right, Func<ulong, ulong, ulong> Convert)
        {
            ArgumentNullException.ThrowIfNull(Left);
            ArgumentNullException.ThrowIfNull(Right);
            ArgumentNullException.ThrowIfNull(Convert);

            var ResultLength = Math.Max(Left.Length, Right.Length);
            var ResultGroupCount = GetGroupCount(ResultLength);
            var Result = new ulong[ResultGroupCount];

            for (var Group = 0; Group < ResultGroupCount; Group++)
            {
                var LeftWord  = Group < Left.Bits.Length  ? Left.Bits[Group]  : 0UL;
                var RightWord = Group < Right.Bits.Length ? Right.Bits[Group] : 0UL;
                Result[Group] = Convert(LeftWord, RightWord);
            }

            return new BitArray(ResultLength, Result, TakeOwnership: true);
        }

        private static void MaskRemainder(ulong[] Buffer, int Length)
        {
            var RemainderBitCount = Length & RemainderFilter;

            if (RemainderBitCount > 0 && Buffer.Length > 0)
            {
                Buffer[^1] &= (1UL << RemainderBitCount) - 1;
            }
        }
        #endregion
    }
}