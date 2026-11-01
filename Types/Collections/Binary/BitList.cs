using Zion.Serialization;

namespace Zion
{
    public sealed class BitList : BitCollection, IList<bool>, IBinarySerializable<BitList>, IParsable<BitList>
    {
        #region Properties
        public int Count { get; private set; }

        public override int Length => Count;

        public bool IsReadOnly => false;
        #endregion

        #region Constructors
        public BitList(int Capacity) : base(Capacity) { }

        public BitList(IEnumerable<ulong> Data, int Count) : base(0)
        {
            ArgumentNullException.ThrowIfNull(Data);
            ArgumentOutOfRangeException.ThrowIfNegative(Count);

            var GroupCount = GetGroupCount(Count);
            var Buffer = Data.ToArray();

            if (Buffer.Length < GroupCount)
            {
                System.Array.Resize(ref Buffer, GroupCount);
            }

            MaskRemainder(Buffer, Count);

            Bits = Buffer;
            this.Count = Count;
        }

        public BitList(IEnumerable<bool> Source) : base(0)
        {
            InitializeFrom(Source, out var Length);
            Count = Length;
        }

        private BitList(int Count, ulong[] Bits, bool TakeOwnership) : base(0)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Count);
            ArgumentNullException.ThrowIfNull(Bits);

            var GroupCount = GetGroupCount(Count);

            if (Bits.Length < GroupCount)
            {
                throw new ArgumentException
                (
                    "Bits length is smaller than the required group count for the given count.",
                    nameof(Bits)
                );
            }

            this.Count = Count;

            if (TakeOwnership && Bits.Length == GroupCount)
            {
                this.Bits = Bits;
                MaskRemainder(this.Bits, Count);
            }
            else
            {
                var Copy = new ulong[GroupCount];
                System.Array.Copy(Bits, Copy, GroupCount);
                MaskRemainder(Copy, Count);
                this.Bits = Copy;
            }
        }
        #endregion

        #region OverrideMethods
        protected override void OnCleared()
        {
            Count = 0;
        }
        #endregion

        #region IList
        public void Add(bool Item)
        {
            var Index = Count;
            EnsureCapacity(Count + 1);

            var Group = Index >> BinaryGroupSize;
            Bits[Group] = Bits[Group].SetBit(Index & RemainderFilter, Item);

            Count++;
        }

        public void Insert(int Index, bool Item)
        {
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Count);

            EnsureCapacity(Count + 1);

            if (Index == Count)
            {
                Add(Item);
                return;
            }

            var OldCount = Count;
            var LastGroup = (OldCount - 1) >> BinaryGroupSize;
            var InsertGroup = Index >> BinaryGroupSize;
            var InsertBit = Index & RemainderFilter;

            var CarryIn = Item;

            for (var Group = InsertGroup; Group <= LastGroup; Group++)
            {
                var StartBit = Group == InsertGroup ? InsertBit : 0;
                var EndBit = Group == LastGroup ? (OldCount - 1) & RemainderFilter : RemainderFilter;

                var Word = Bits[Group];
                var Shifted = ShiftLeftInRange(Word, StartBit, EndBit, CarryIn, out var CarryOut);
                Bits[Group] = Shifted;
                CarryIn = CarryOut;
            }

            Count = OldCount + 1;
        }

        public void RemoveAt(int Index)
        {
            if (Index < 0 || Index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(Index), $"Index(={Index}) out of range [0..{Count})");
            }

            var OldCount = Count;
            var LastGroup = (OldCount - 1) >> BinaryGroupSize;
            var RemoveGroup = Index >> BinaryGroupSize;
            var RemoveBit = Index & RemainderFilter;

            var CarryIn = false;

            for (var Group = LastGroup; Group >= RemoveGroup; Group--)
            {
                var StartBit = Group == RemoveGroup ? RemoveBit : 0;
                var EndBit = Group == LastGroup ? (OldCount - 1) & RemainderFilter : RemainderFilter;

                var Word = Bits[Group];
                var Shifted = ShiftRightInRange(Word, StartBit, EndBit, CarryIn, out var CarryOut);
                Bits[Group] = Shifted;
                CarryIn = CarryOut;
            }

            Count = OldCount - 1;

            var ClearGroup = Count >> BinaryGroupSize;
            var ClearBit = Count & RemainderFilter;
            Bits[ClearGroup] = Bits[ClearGroup].SetBit(ClearBit, false);
        }

        public bool Remove(bool Item)
        {
            var Index = IndexOf(Item);

            if (Index >= 0)
            {
                RemoveAt(Index);
                return true;
            }

            return false;
        }
        
        public void CopyTo(bool[] Array, int ArrayIndex)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfNegative(ArrayIndex);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(ArrayIndex, Array.Length);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(Count, Array.Length - ArrayIndex);

            for (var Index = 0; Index < Count; Index++)
            {
                Array[ArrayIndex + Index] = this[Index];
            }
        }
        #endregion

        #region PublicMethods
        public BitList Clone()
        {
            var Copy = new ulong[Bits.Length];
            System.Array.Copy(Bits, Copy, Bits.Length);
            return new BitList(Count, Copy, TakeOwnership: true);
        }

        public void EnsureCapacity(int Capacity)
        {
            if (Capacity <= this.Capacity)
            {
                return;
            }

            var GroupCount = GetGroupCount(Capacity);
            var NewData = new ulong[GroupCount];
            System.Array.Copy(Bits, NewData, Math.Min(Bits.Length, GroupCount));
            Bits = NewData;
        }

        public BitArray ToBitArray()
        {
            var Copy = new ulong[Bits.Length];
            System.Array.Copy(Bits, Copy, Bits.Length);
            return new BitArray(Copy, Count);
        }
        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer)
        {
            ArgumentNullException.ThrowIfNull(Writer);

            Writer.Write(Count);
            Writer.BaseStream.Write(AsByteSpan().Slice(0, GetByteCount()));
        }

        public static BitList Read(BinaryReader Reader)
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

            return new BitList(Count, Buffer, TakeOwnership: true);
        }
        #endregion

        #region IParsable
        public static BitList Parse(string String, IFormatProvider? Provider)
        {
            var (Data, Count) = ParseBitCollection(String);
            return new BitList(Data, Count);
        }

        public static bool TryParse(string? String, IFormatProvider? Provider, out BitList Result)
        {
            if (TryParseBitCollection(String, out var Data, out var Count))
            {
                Result = new BitList(Data, Count);
                return true;
            }

            Result = null!;
            return false;
        }
        #endregion

        #region PrivateMethods
        private static void MaskRemainder(ulong[] Buffer, int Count)
        {
            var RemainderBitCount = Count & RemainderFilter;

            if (RemainderBitCount > 0 && Buffer.Length > 0)
            {
                Buffer[^1] &= (1UL << RemainderBitCount) - 1;
            }
        }

        private static ulong ShiftLeftInRange(ulong Value, int StartBit, int EndBit, bool CarryIn, out bool CarryOut)
        {
            var Width = EndBit - StartBit + 1;
            var RangeMask = Width == GroupSize
                ? ~0UL
                : ((1UL << Width) - 1) << StartBit;

            var LowerMask = Width == 1
                ? 0UL
                : ((1UL << (Width - 1)) - 1) << StartBit;

            var Shifted = (Value & LowerMask) << 1;
            var Preserved = Value & ~RangeMask;

            CarryOut = (Value & (1UL << EndBit)) != 0;

            var Result = Preserved | Shifted;

            if (CarryIn)
            {
                Result |= 1UL << StartBit;
            }

            return Result;
        }

        private static ulong ShiftRightInRange(ulong Value, int StartBit, int EndBit, bool CarryIn, out bool CarryOut)
        {
            var Width = EndBit - StartBit + 1;
            var RangeMask = Width == GroupSize
                ? ~0UL
                : ((1UL << Width) - 1) << StartBit;

            var UpperMask = Width == 1
                ? 0UL
                : ((1UL << (Width - 1)) - 1) << (StartBit + 1);

            var Shifted = (Value & UpperMask) >> 1;
            var Preserved = Value & ~RangeMask;

            CarryOut = (Value & (1UL << StartBit)) != 0;

            var Result = Preserved | Shifted;

            if (CarryIn)
            {
                Result |= 1UL << EndBit;
            }

            return Result;
        }
        #endregion
    }
}