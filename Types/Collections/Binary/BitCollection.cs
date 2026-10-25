using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Zion
{
    public abstract class BitCollection : IEnumerable<bool>, IEqualityComparer<BitCollection>
    {
        #region Constants
        protected const int GroupSize        = sizeof(ulong);
        protected const int BinaryGroupSize  = 6;
        protected const int RemainderFilter  = GroupSize - 1;
        protected const int GroupCountFilter = ~RemainderFilter;

        #endregion

        #region Data & Properties
        protected ulong[] Data;

        public abstract int Count { get; }

        #endregion

        #region Indexers
        public bool this[int Index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);
                return Data[Index >> BinaryGroupSize].GetBit(Index & RemainderFilter);
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);
                int Group = Index >> BinaryGroupSize;
                Data[Group] = Data[Group].SetBit(Index & RemainderFilter, value);
            }
        }

        public bool this[Index Index]
        {
            get => this[Index.GetOffset(Count)];
            set => this[Index.GetOffset(Count)] = value;
        }

        #endregion

        #region Operators
        public static bool operator ==(BitCollection? A, BitCollection? B)
        {
            if (object.CompareReferences(A, B, out bool ReferenceComprasion))
            {
                return ReferenceComprasion;
            }

            if (A.Count != B.Count)
            {
                return false;
            }

            int TotalBits = A.Count;

            int FullGroups = TotalBits >> BinaryGroupSize;
            int Remainders = TotalBits & RemainderFilter;

            if (FullGroups > 0)
            {
                var ASpan = A.Data.AsSpan(0, FullGroups);
                var BSpan = B.Data.AsSpan(0, FullGroups);

                if (!ASpan.SequenceEqual(BSpan))
                {
                    return false;
                }
            }

            if (Remainders > 0)
            {
                int Shift = GroupSize - Remainders;
                return A.Data[FullGroups] >> Shift
                    == B.Data[FullGroups] >> Shift;
            }

            return true;
        }

        public static bool operator !=(BitCollection? A, BitCollection? B)
        {
            return !(A == B);
        }

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            int TotalCount = Count + GetByteCount() + 1;

            return string.Create
            (
                TotalCount,
                (this, Count),
                static (Span, Data) =>
                {
                    var Source = Data.Item1.AsByteSpan();
                    var Count  = Data.Count;

                    int ByteCount = Count >> 3;
                    int Remainders = Count & 0b111;

                    int Index = 1;

                    for (int i = 0; i < ByteCount; i++)
                    {
                        foreach (bool Bit in Source[i].EnumerateBits())
                        {
                            Span[Index++] = Bit ? '1' : '0';
                        }
                        Span[Index++] = '_';
                    }

                    if (Remainders > 0)
                    {
                        foreach (bool Bit in Source[^1].EnumerateBits(Remainders))
                        {
                            Span[Index++] = Bit ? '1' : '0';
                        }
                    }

                    Span[0] = '[';
                    Span[^1] = ']';
                }
            );
        }

        public override bool Equals(object? Object)
        {
            return Object is BitCollection BitCollection && this == BitCollection;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data, Count);
        }

        #endregion

        #region StaticMethods
        public static int GetByteCount(int Count)
        {
            return (Count + 7) >> 3;
        }

        public static int GetGroupCount(int Count)
        {
            return (Count + RemainderFilter) >> BinaryGroupSize;
        }

        #endregion

        #region PublicMethods
        public int GetByteCount()
        {
            return GetByteCount(Count);
        }

        public int GetGroupCount()
        {
            return GetGroupCount(Count);
        }
        

        public Span<byte> AsByteSpan()
        {
            return MemoryMarshal.Cast<ulong, byte>(Data.AsSpan());
        }

        public Span<ushort> AsUInt16Span()
        {
            return MemoryMarshal.Cast<ulong, ushort>(Data.AsSpan());
        }

        public Span<uint> AsUInt32Span()
        {
            return MemoryMarshal.Cast<ulong, uint>(Data.AsSpan());
        }

        public Span<ulong> AsSpan()
        {
            return Data.AsSpan();
        }


        public int IndexOf(bool Item)
        {
            var Span = AsSpan();
            int Count = this.Count;

            int GroupCount = Count >> BinaryGroupSize;

            for (int Group = 0; Group < GroupCount; Group++)
            {
                ulong Current = Span[Group];

                if (!Item)
                {
                    Current = ~Current;
                }

                if (Current != 0UL)
                {
                    int BitIndex = BitOperations.TrailingZeroCount(Current);
                    return (Group << BinaryGroupSize) | BitIndex;
                }
            }

            int RemainderBits = Count & RemainderFilter;

            if (RemainderBits != 0)
            {
                ulong Last = Span[GroupCount];

                if (!Item)
                {
                    Last = ~Last;
                }

                Last &= (1UL << RemainderBits) - 1;

                if (Last != 0UL)
                {
                    int BitIndex = BitOperations.TrailingZeroCount(Last);
                    return (GroupCount << BinaryGroupSize) | BitIndex;
                }
            }

            return -1;
        }

        public bool Contains(bool Item)
        {
            return IndexOf(Item) != -1;
        }

        public bool Contains(int Start, int Count, bool Target)
        {
            int Length = this.Count;

            ArgumentOutOfRangeException.ThrowIfWithout(Start, Length);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, Length);

            int EndBit = Start + Count;

            int FirstWordIndex  = Start >> BinaryGroupSize;
            int LastWordIndex   = (EndBit - 1) >> BinaryGroupSize;
            int FirstBitOffset  = Start & RemainderFilter;
            int LastGroupLength = EndBit & RemainderFilter;

            ulong FirstMask = ~0UL << FirstBitOffset;
            ulong LastMask = LastGroupLength == 0 ? ~0UL : (1UL << LastGroupLength) - 1;

            if (FirstWordIndex == LastWordIndex)
            {
                ulong Mask = FirstMask & LastMask;
                ulong Current = Data[FirstWordIndex] & Mask;

                return Target ? Current != 0 : Current != Mask;
            }

            ulong FirstWord = Data[FirstWordIndex] & FirstMask;
            if (Target ? FirstWord != 0 : FirstWord != FirstMask)
            {
                return true;
            }

            Span<ulong> Span = AsSpan();
            for (int i = FirstWordIndex + 1; i < LastWordIndex; i++)
            {
                ulong Current = Span[i];

                if (Target ? Current != 0 : Current != ~0UL)
                {
                    return true;
                }
            }

            ulong Last = Data[LastWordIndex] & LastMask;
            return Target ? Last != 0 : Last != LastMask;
        }


        public void Fill(bool Value)
        {
            Fill(0, Count, Value);
        }

        public void Fill(Segment Range, bool Value)
        {
            Fill(Range.Start, Range.Length, Value);
        }
        
        public void Fill(int Start, int Count, bool Value)
        {
            int Length = this.Count;

            ArgumentOutOfRangeException.ThrowIfWithout(Start, Length);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, Length);

            int EndBit = Start + Count;

            int FirstWordIndex = Start >> BinaryGroupSize;
            int LastWordIndex  = (EndBit - 1) >> BinaryGroupSize;
            int FirstBitOffset = Start & RemainderFilter;
            int LastBitCount   = EndBit & RemainderFilter;

            ulong FirstMask = ~0UL << FirstBitOffset;
            ulong LastMask = LastBitCount == 0 ? ~0UL : (1UL << LastBitCount) - 1;

            if (FirstWordIndex == LastWordIndex)
            {
                ulong Mask = FirstMask & LastMask;

                if (Value)
                {
                    Data[FirstWordIndex] |= Mask;
                }
                else
                {
                    Data[FirstWordIndex] &= ~Mask;
                }
                return;
            }

            if (Value)
            {
                Data[FirstWordIndex] |= FirstMask;
            }
            else
            {
                Data[FirstWordIndex] &= ~FirstMask;
            }

            Span<ulong> Span = AsSpan();
            ulong FillValue = Value ? ~0UL : 0UL;

            for (int i = FirstWordIndex + 1; i < LastWordIndex; i++)
            {
                Span[i] = FillValue;
            }

            if (Value)
            {
                Data[LastWordIndex] |= LastMask;
            }
            else
            {
                Data[LastWordIndex] &= ~LastMask;
            }
        }

        public bool TryFindShortestSequence(int BitCount, bool TargetBit, out Segment Sequence)
        {
            return TryFindShortestSequence(BitCount, TargetBit, Count, out Sequence);
        }

        public bool TryFindShortestSequence(int BitCount, bool TargetBit, int Count, out Segment Sequence)
        {
            return TryFindShortestSequence(BitCount, TargetBit, 0, Count, out Sequence);
        }

        public bool TryFindShortestSequence(int BitCount, bool TargetBit, int Start, int Count, out Segment Sequence)
        {
            int CollectionLength = this.Count;

            ArgumentOutOfRangeException.ThrowIfNegative(BitCount);
            ArgumentOutOfRangeException.ThrowIfWithout(Start, CollectionLength);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, CollectionLength);

            Sequence = Segment.Empty;

            if (BitCount == 0)
            {
                return true;
            }

            Span<ulong> Data = AsSpan();

            int EndBit = Start + Count;

            int FirstWordIndex = Start >> BinaryGroupSize;
            int LastWordIndex = (EndBit - 1) >> BinaryGroupSize;
            int FirstBitOffset = Start & RemainderFilter;
            int LastBitCount = EndBit & RemainderFilter;

            if (LastBitCount == 0)
            {
                LastBitCount = 64;
            }

            ulong FirstMask = ~0UL << FirstBitOffset;
            ulong LastMask = (1UL << LastBitCount) - 1;

            int CurrentStart = -1;
            int CurrentLength = 0;

            int BestStart = -1;
            int BestLength = int.MaxValue;

            bool TryUpdateBest()
            {
                if (CurrentLength >= BitCount && CurrentLength < BestLength)
                {
                    BestLength = CurrentLength;
                    BestStart = CurrentStart;

                    if (BestLength == BitCount)
                    {
                        return true;
                    }
                }
                return false;
            }

            void ResetCurrent()
            {
                CurrentLength = 0;
                CurrentStart = -1;
            }

            for (int WordIndex = FirstWordIndex; WordIndex <= LastWordIndex; WordIndex++)
            {
                ulong Word = Data[WordIndex];

                if (WordIndex == FirstWordIndex) Word &= FirstMask;
                if (WordIndex == LastWordIndex) Word &= LastMask;

                if (!TargetBit) Word = ~Word;

                if (Word == 0)
                {
                    if (CurrentLength > 0)
                    {
                        if (TryUpdateBest())
                        {
                            Sequence = new Segment(BestStart, BestLength);
                            return true;
                        }
                        ResetCurrent();
                    }
                    continue;
                }

                var WordStartBit = WordIndex << BinaryGroupSize;
                var Remaining = Word;

                while (Remaining != 0)
                {
                    int SegmentOffset = BitOperations.TrailingZeroCount(Remaining);
                    int SegmentLength = BitOperations.LeadingZeroCount(~(Remaining >> SegmentOffset));

                    if (CurrentLength > 0 && SegmentOffset == 0 &&
                        (WordIndex > FirstWordIndex || FirstBitOffset == 0 || CurrentLength > 0))
                    {
                        CurrentLength += SegmentLength;
                    }
                    else
                    {
                        CurrentStart = WordStartBit + SegmentOffset;
                        CurrentLength = SegmentLength;
                    }

                    if (TryUpdateBest())
                    {
                        Sequence = new Segment(BestStart, BestLength);
                        return true;
                    }

                    int TotalShift = SegmentOffset + SegmentLength;
                    Remaining = TotalShift >= 64 ? 0 : Remaining >> TotalShift;
                }

                if ((Word & 1UL) == 0 && CurrentLength > 0)
                {
                    if (TryUpdateBest())
                    {
                        Sequence = new Segment(BestStart, BestLength);
                        return true;
                    }
                    ResetCurrent();
                }
            }

            if (CurrentLength >= BitCount && CurrentLength < BestLength)
            {
                BestLength = CurrentLength;
                BestStart = CurrentStart;
            }

            if (BestStart != -1)
            {
                Sequence = new Segment(BestStart, BestLength);
                return true;
            }

            return false;
        }


        public byte[] ToByteArray()
        {
            int Count = this.Count;
            int ByteCount = GetByteCount(Count);

            byte[] Result = new byte[ByteCount];

            Array.Copy(Data, Result, ByteCount);

            return Result;
        }

        public bool[] ToBooleanArray()
        {
            bool[] Result = new bool[Count];

            int Index = 0;

            foreach (bool Bit in this)
            {
                if (Bit)
                {
                    Result[Index] = true;
                }

                Index++;
            }

            return Result;
        }

        #endregion

        #region IParsable
        public static (List<ulong> Data, int Count) ParseBitCollection(string String)
        {
            return TryParseBitCollection(String.NotNull(), out var Data, out var Count)
                ? (Data, Count)
                : throw new FormatException("The string has an invalid format for parsing");
        }

        public static bool TryParseBitCollection(string? String, out List<ulong> Data, out int Count)
        {
            if (String is null)
            {
                Data  = default!;
                Count = default!;
                return false;
            }

            Data = new List<ulong>();
            Count = 0;
            var Current = 0UL;

            foreach (char Char in String)
            {
                if (Char == '1')
                {
                    Current |= 1UL;
                }
                else if (Char != '0')
                {
                    return false;
                }

                Current <<= 1;
                Count++;

                if ((Count & RemainderFilter) == 0)
                {
                    Data.Add(Current);
                    Current = 0UL;
                }
            }

            if ((Count & RemainderFilter) == 0)
            {
                Data.Add(Current);
            }

            return true;
        }

        #endregion

        #region IEqualityComparer
        public bool Equals(BitCollection? X, BitCollection? Y)
        {
            return X == Y;
        }

        public int GetHashCode(BitCollection Object)
        {
            return Object.GetHashCode();
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<bool> GetEnumerator()
        {
            var Source = Data;
            var Count = this.Count;

            int ByteCount = Count >> 3;
            int Remainders = Count & 0b111;

            for (int i = 0; i < ByteCount; i++)
            {
                foreach (bool Bit in Source[i].EnumerateBits())
                {
                    yield return Bit;
                }
            }

            if (Remainders > 0)
            {
                foreach (bool Bit in Source[^1].EnumerateBits(Remainders))
                {
                    yield return Bit;
                }
            }
        }

        #endregion
    }
}