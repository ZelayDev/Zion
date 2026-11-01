using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Zion
{
    public abstract class BitCollection : IEnumerable<bool>, IEqualityComparer<BitCollection>
    {
        #region Constants
        protected const int BinaryGroupSize = 6;
        protected const int GroupSize       = 1 << BinaryGroupSize;
        protected const int RemainderFilter = GroupSize - 1;
        #endregion

        #region Data & Properties
        protected ulong[] Bits;

        public int Capacity => Bits.Length << BinaryGroupSize;

        public abstract int Length { get; }
        #endregion

        #region Constructors
        protected BitCollection(int Capacity)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);
            Bits = new ulong[GetGroupCount(Capacity)];
        }

        protected internal BitCollection(int Capacity, ulong[] Data)
        {
            ArgumentNullException.ThrowIfNull(Data);
            ArgumentOutOfRangeException.ThrowIfNegative(Capacity);

            var RequiredGroupCount = GetGroupCount(Capacity);

            if (Data.Length < RequiredGroupCount)
            {
                throw new ArgumentException
                (
                    "Data length is smaller than the required group count for the given capacity.",
                    nameof(Data)
                );
            }

            Bits = Data;
        }
        #endregion

        #region Indexers
        public bool this[int Index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Length);
                return Bits[Index >> BinaryGroupSize].GetBit(Index & RemainderFilter);
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Length);
                var Group = Index >> BinaryGroupSize;
                Bits[Group] = Bits[Group].SetBit(Index & RemainderFilter, value);
            }
        }

        public bool this[Index Index]
        {
            get => this[Index.GetOffset(Length)];
            set => this[Index.GetOffset(Length)] = value;
        }
        #endregion

        #region Operators
        public static bool operator ==(BitCollection? Left, BitCollection? Right)
        {
            if (Object.CompareReferences(Left, Right, out var ReferenceComparison))
            {
                return ReferenceComparison;
            }

            if (Left!.Length != Right!.Length)
            {
                return false;
            }

            var TotalBitCount = Left.Length;
            var FullGroupCount = TotalBitCount >> BinaryGroupSize;
            var RemainderBitCount = TotalBitCount & RemainderFilter;

            if (FullGroupCount > 0)
            {
                var LeftSpan = Left.Bits.AsSpan(0, FullGroupCount);
                var RightSpan = Right.Bits.AsSpan(0, FullGroupCount);

                if (!LeftSpan.SequenceEqual(RightSpan))
                {
                    return false;
                }
            }

            if (RemainderBitCount > 0)
            {
                var Mask = (1UL << RemainderBitCount) - 1;
                return (Left.Bits[FullGroupCount] & Mask) == (Right.Bits[FullGroupCount] & Mask);
            }

            return true;
        }

        public static bool operator !=(BitCollection? Left, BitCollection? Right)
        {
            return !(Left == Right);
        }
        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            var BitCount = Length;
            var ByteCount = GetByteCount();

            var FullByteCount = BitCount >> 3;
            var RemainderBitCount = BitCount & 0b111;

            var SeparatorCount = RemainderBitCount > 0 ? FullByteCount : FullByteCount - 1;

            if (SeparatorCount < 0)
            {
                SeparatorCount = 0;
            }

            var TotalCharacterCount = BitCount + SeparatorCount + 2;

            return String.Create
            (
                TotalCharacterCount,
                (Collection: this, BitCount, FullByteCount, RemainderBitCount),
                static (Span, State) =>
                {
                    var Source = State.Collection.AsByteSpan();
                    var Index = 1;

                    for (var ByteIndex = 0; ByteIndex < State.FullByteCount; ByteIndex++)
                    {
                        foreach (var Bit in Source[ByteIndex].EnumerateBits())
                        {
                            Span[Index++] = Bit ? '1' : '0';
                        }

                        if (ByteIndex < State.FullByteCount - 1 || State.RemainderBitCount > 0)
                        {
                            Span[Index++] = '_';
                        }
                    }

                    if (State.RemainderBitCount > 0)
                    {
                        foreach (var Bit in Source[State.FullByteCount].EnumerateBits(State.RemainderBitCount))
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
            var Hash = new HashCode();
            Hash.Add(Length);

            var TotalBitCount = Length;
            var FullGroupCount = TotalBitCount >> BinaryGroupSize;
            var RemainderBitCount = TotalBitCount & RemainderFilter;

            for (var Group = 0; Group < FullGroupCount; Group++)
            {
                Hash.Add(Bits[Group]);
            }

            if (RemainderBitCount > 0)
            {
                var Mask = (1UL << RemainderBitCount) - 1;
                Hash.Add(Bits[FullGroupCount] & Mask);
            }

            return Hash.ToHashCode();
        }
        #endregion

        #region AbstractMethods
        protected virtual void OnCleared() { }
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
            return GetByteCount(Length);
        }

        public int GetGroupCount()
        {
            return GetGroupCount(Length);
        }


        public Span<byte> AsByteSpan()
        {
            return MemoryMarshal.Cast<ulong, byte>(AsSpan());
        }

        public Span<short> AsInt16Span()
        {
            return MemoryMarshal.Cast<ulong, short>(AsSpan());
        }

        public Span<ushort> AsUInt16Span()
        {
            return MemoryMarshal.Cast<ulong, ushort>(AsSpan());
        }

        public Span<int> AsInt32Span()
        {
            return MemoryMarshal.Cast<ulong, int>(AsSpan());
        }

        public Span<uint> AsUInt32Span()
        {
            return MemoryMarshal.Cast<ulong, uint>(AsSpan());
        }

        public Span<long> AsInt64Span()
        {
            return MemoryMarshal.Cast<ulong, long>(AsSpan());
        }

        public Span<ulong> AsSpan()
        {
            return Bits.AsSpan();
        }


        public int IndexOf(bool Item)
        {
            var BitCount = Length;
            var FullGroupCount = BitCount >> BinaryGroupSize;
            var RemainderBitCount = BitCount & RemainderFilter;

            for (var Group = 0; Group < FullGroupCount; Group++)
            {
                var Current = Bits[Group];

                if (!Item)
                {
                    Current = ~Current;
                }

                if (Current != 0UL)
                {
                    var BitIndex = BitOperations.TrailingZeroCount(Current);
                    return (Group << BinaryGroupSize) | BitIndex;
                }
            }

            if (RemainderBitCount > 0)
            {
                var Current = Bits[FullGroupCount];

                if (!Item)
                {
                    Current = ~Current;
                }

                var Mask = (1UL << RemainderBitCount) - 1;
                Current &= Mask;

                if (Current != 0UL)
                {
                    var BitIndex = BitOperations.TrailingZeroCount(Current);
                    return (FullGroupCount << BinaryGroupSize) | BitIndex;
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
            var TotalLength = Length;

            ArgumentOutOfRangeException.ThrowIfWithout(Start, TotalLength);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, TotalLength);

            if (Count == 0)
            {
                return false;
            }

            var EndBit = Start + Count;

            var FirstGroupIndex = Start >> BinaryGroupSize;
            var LastGroupIndex = (EndBit - 1) >> BinaryGroupSize;
            var FirstBitOffset = Start & RemainderFilter;
            var LastBitCount = EndBit & RemainderFilter;

            var FirstMask = ~0UL << FirstBitOffset;
            var LastMask = LastBitCount == 0 ? ~0UL : (1UL << LastBitCount) - 1;

            if (FirstGroupIndex == LastGroupIndex)
            {
                var Mask = FirstMask & LastMask;
                var Current = Bits[FirstGroupIndex] & Mask;

                return Target ? Current != 0 : Current != Mask;
            }

            var FirstGroupValue = Bits[FirstGroupIndex] & FirstMask;
            if (Target ? FirstGroupValue != 0 : FirstGroupValue != FirstMask)
            {
                return true;
            }

            for (var Group = FirstGroupIndex + 1; Group < LastGroupIndex; Group++)
            {
                var Current = Bits[Group];

                if (Target ? Current != 0 : Current != ~0UL)
                {
                    return true;
                }
            }

            var LastGroupValue = Bits[LastGroupIndex] & LastMask;
            return Target ? LastGroupValue != 0 : LastGroupValue != LastMask;
        }


        public void Clear()
        {
            Array.Clear(Bits, 0, GetGroupCount(Length));
            OnCleared();
        }


        public void Fill(bool Value)
        {
            Fill(0, Length, Value);
        }

        public void Fill(Segment Range, bool Value)
        {
            Fill(Range.Start, Range.Length, Value);
        }

        public void Fill(int Start, int Count, bool Value)
        {
            var TotalLength = Length;

            ArgumentOutOfRangeException.ThrowIfWithout(Start, TotalLength);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, TotalLength);

            if (Count == 0)
            {
                return;
            }

            var EndBit = Start + Count;

            var FirstGroupIndex = Start >> BinaryGroupSize;
            var LastGroupIndex = (EndBit - 1) >> BinaryGroupSize;
            var FirstBitOffset = Start & RemainderFilter;
            var LastBitCount = EndBit & RemainderFilter;

            var FirstMask = ~0UL << FirstBitOffset;
            var LastMask = LastBitCount == 0 ? ~0UL : (1UL << LastBitCount) - 1;

            if (FirstGroupIndex == LastGroupIndex)
            {
                var Mask = FirstMask & LastMask;

                if (Value)
                {
                    Bits[FirstGroupIndex] |= Mask;
                }
                else
                {
                    Bits[FirstGroupIndex] &= ~Mask;
                }
                return;
            }

            if (Value)
            {
                Bits[FirstGroupIndex] |= FirstMask;
            }
            else
            {
                Bits[FirstGroupIndex] &= ~FirstMask;
            }

            var FillValue = Value ? ~0UL : 0UL;

            for (var Group = FirstGroupIndex + 1; Group < LastGroupIndex; Group++)
            {
                Bits[Group] = FillValue;
            }

            if (Value)
            {
                Bits[LastGroupIndex] |= LastMask;
            }
            else
            {
                Bits[LastGroupIndex] &= ~LastMask;
            }
        }


        public bool TryFindShortestSequence(int BitCount, bool TargetBit, out Segment Sequence)
        {
            return TryFindShortestSequence(BitCount, TargetBit, 0, Length, out Sequence);
        }

        public bool TryFindShortestSequence(int BitCount, bool TargetBit, int Count, out Segment Sequence)
        {
            return TryFindShortestSequence(BitCount, TargetBit, 0, Count, out Sequence);
        }

        public bool TryFindShortestSequence(int BitCount, bool TargetBit, int Start, int Count, out Segment Sequence)
        {
            var TotalLength = Length;

            ArgumentOutOfRangeException.ThrowIfNegative(BitCount);
            ArgumentOutOfRangeException.ThrowIfWithout(Start, TotalLength);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, TotalLength);

            Sequence = Segment.Empty;

            if (BitCount == 0)
            {
                return true;
            }

            if (Count < BitCount)
            {
                return false;
            }

            var EndBit = Start + Count;

            var FirstGroupIndex = Start >> BinaryGroupSize;
            var LastGroupIndex = (EndBit - 1) >> BinaryGroupSize;
            var FirstBitOffset = Start & RemainderFilter;
            var LastBitCount = EndBit & RemainderFilter;

            var FirstMask = ~0UL << FirstBitOffset;
            var LastMask = LastBitCount == 0 ? ~0UL : (1UL << LastBitCount) - 1;

            var BestStart = -1;
            var BestLength = Int32.MaxValue;

            var CurrentStart = -1;
            var CurrentLength = 0;

            for (var GroupIndex = FirstGroupIndex; GroupIndex <= LastGroupIndex; GroupIndex++)
            {
                if (BestLength != Int32.MaxValue)
                {
                    var RemainingBitCount = EndBit - (GroupIndex << BinaryGroupSize);
                    if (RemainingBitCount < BestLength)
                    {
                        break;
                    }
                }

                var Word = Bits[GroupIndex];

                if (GroupIndex == FirstGroupIndex)
                {
                    Word &= FirstMask;
                }

                if (GroupIndex == LastGroupIndex)
                {
                    Word &= LastMask;
                }

                if (!TargetBit)
                {
                    Word = ~Word;
                }

                var GroupStartBit = GroupIndex << BinaryGroupSize;

                if (Word == 0UL)
                {
                    if (CurrentLength > 0)
                    {
                        if (CurrentLength >= BitCount && CurrentLength < BestLength)
                        {
                            BestLength = CurrentLength;
                            BestStart = CurrentStart;

                            if (BestLength == BitCount)
                            {
                                Sequence = new Segment(BestStart, BestLength);
                                return true;
                            }
                        }

                        CurrentLength = 0;
                        CurrentStart = -1;
                    }

                    continue;
                }

                if (Word == ~0UL)
                {
                    if (CurrentLength == 0)
                    {
                        CurrentStart = GroupStartBit;
                    }

                    CurrentLength += GroupSize;

                    if (CurrentLength >= BitCount && CurrentLength < BestLength)
                    {
                        BestLength = CurrentLength;
                        BestStart = CurrentStart;

                        if (BestLength == BitCount)
                        {
                            Sequence = new Segment(BestStart, BestLength);
                            return true;
                        }
                    }

                    continue;
                }

                var Remaining = Word;

                while (Remaining != 0UL)
                {
                    var SeriesOffset = BitOperations.TrailingZeroCount(Remaining);
                    var Shifted = Remaining >> SeriesOffset;

                    var SeriesLength = BitOperations.LeadingZeroCount(~Shifted);
                    if (SeriesLength == 0)
                    {
                        SeriesLength = GroupSize - SeriesOffset;
                    }

                    if (CurrentLength > 0 && SeriesOffset == 0)
                    {
                        CurrentLength += SeriesLength;
                    }
                    else
                    {
                        CurrentStart = GroupStartBit + SeriesOffset;
                        CurrentLength = SeriesLength;
                    }

                    if (CurrentLength >= BitCount && CurrentLength < BestLength)
                    {
                        BestLength = CurrentLength;
                        BestStart = CurrentStart;

                        if (BestLength == BitCount)
                        {
                            Sequence = new Segment(BestStart, BestLength);
                            return true;
                        }
                    }

                    var ConsumedBits = SeriesOffset + SeriesLength;
                    Remaining = ConsumedBits >= GroupSize ? 0UL : Remaining >> ConsumedBits;
                }

                if (Word >> (GroupSize - 1) == 0UL && CurrentLength > 0)
                {
                    if (CurrentLength >= BitCount && CurrentLength < BestLength)
                    {
                        BestLength = CurrentLength;
                        BestStart = CurrentStart;

                        if (BestLength == BitCount)
                        {
                            Sequence = new Segment(BestStart, BestLength);
                            return true;
                        }
                    }

                    CurrentLength = 0;
                    CurrentStart = -1;
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


        public bool TryFindFirstFit(int BitCount, bool TargetBit, out Segment Sequence)
        {
            return TryFindFirstFit(BitCount, TargetBit, 0, Length, out Sequence);
        }

        public bool TryFindFirstFit(int BitCount, bool TargetBit, int Count, out Segment Sequence)
        {
            return TryFindFirstFit(BitCount, TargetBit, 0, Count, out Sequence);
        }

        public bool TryFindFirstFit(int BitCount, bool TargetBit, int Start, int Count, out Segment Sequence)
        {
            var TotalLength = Length;

            ArgumentOutOfRangeException.ThrowIfNegative(BitCount);
            ArgumentOutOfRangeException.ThrowIfWithout(Start, TotalLength);
            ArgumentOutOfRangeException.ThrowIfWithout(Start + Count, TotalLength);

            Sequence = Segment.Empty;

            if (BitCount == 0)
            {
                return true;
            }

            if (Count < BitCount)
            {
                return false;
            }

            var EndBit = Start + Count;

            var FirstGroupIndex = Start >> BinaryGroupSize;
            var LastGroupIndex = (EndBit - 1) >> BinaryGroupSize;
            var FirstBitOffset = Start & RemainderFilter;
            var LastBitCount = EndBit & RemainderFilter;

            var FirstMask = ~0UL << FirstBitOffset;
            var LastMask = LastBitCount == 0 ? ~0UL : (1UL << LastBitCount) - 1;

            var CurrentStart = -1;
            var CurrentLength = 0;

            for (var GroupIndex = FirstGroupIndex; GroupIndex <= LastGroupIndex; GroupIndex++)
            {
                var Word = Bits[GroupIndex];

                if (GroupIndex == FirstGroupIndex)
                {
                    Word &= FirstMask;
                }
                if (GroupIndex == LastGroupIndex)
                {
                    Word &= LastMask;
                }

                if (!TargetBit)
                {
                    Word = ~Word;
                }

                var GroupStartBit = GroupIndex << BinaryGroupSize;

                if (Word == 0UL)
                {
                    CurrentLength = 0;
                    CurrentStart = -1;
                    continue;
                }

                if (Word == ~0UL)
                {
                    if (CurrentLength == 0)
                    {
                        CurrentStart = GroupStartBit;
                    }
                    CurrentLength += GroupSize;

                    if (CurrentLength >= BitCount)
                    {
                        Sequence = new Segment(CurrentStart, CurrentLength);
                        return true;
                    }
                    continue;
                }

                var Remaining = Word;

                while (Remaining != 0UL)
                {
                    var SeriesOffset = BitOperations.TrailingZeroCount(Remaining);
                    var Shifted = Remaining >> SeriesOffset;
                    var SeriesLength = BitOperations.LeadingZeroCount(~Shifted);
                    if (SeriesLength == 0)
                    {
                        SeriesLength = GroupSize - SeriesOffset;
                    }

                    if (CurrentLength > 0 && SeriesOffset == 0)
                    {
                        CurrentLength += SeriesLength;
                    }
                    else
                    {
                        CurrentStart = GroupStartBit + SeriesOffset;
                        CurrentLength = SeriesLength;
                    }

                    if (CurrentLength >= BitCount)
                    {
                        Sequence = new Segment(CurrentStart, CurrentLength);
                        return true;
                    }

                    var ConsumedBits = SeriesOffset + SeriesLength;
                    Remaining = ConsumedBits >= GroupSize ? 0UL : Remaining >> ConsumedBits;
                }

                if (Word >> (GroupSize - 1) == 0UL)
                {
                    CurrentLength = 0;
                    CurrentStart = -1;
                }
            }

            return false;
        }


        public byte[] ToByteArray()
        {
            var Result = new byte[GetByteCount()];
            AsByteSpan().Slice(0, Result.Length).CopyTo(Result);
            return Result;
        }

        public bool[] ToBooleanArray()
        {
            var Result = new bool[Length];

            for (var Index = 0; Index < Result.Length; Index++)
            {
                Result[Index] = this[Index];
            }

            return Result;
        }
        #endregion

        #region ProtectedMethods
        protected void InitializeFrom(IEnumerable<bool> Source, out int Length)
        {
            ArgumentNullException.ThrowIfNull(Source);

            var Buffer = Source as ICollection<bool> ?? Source.ToArray();

            Length = Buffer.Count;

            var GroupCount = GetGroupCount(Length);
            var Words = new ulong[GroupCount];

            var Index = 0;
            foreach (var Bit in Buffer)
            {
                if (Bit)
                {
                    Words[Index >> BinaryGroupSize] |= 1UL << (Index & RemainderFilter);
                }
                Index++;
            }

            Bits = Words;
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
                Data = default!;
                Count = default!;
                return false;
            }

            Data = new List<ulong>();
            Count = 0;

            var Current = 0UL;

            foreach (var Character in String)
            {
                if (Character == '1')
                {
                    Current |= 1UL << (Count & RemainderFilter);
                }
                else if (Character != '0')
                {
                    Data = default!;
                    Count = default!;
                    return false;
                }

                Count++;

                if ((Count & RemainderFilter) == 0)
                {
                    Data.Add(Current);
                    Current = 0UL;
                }
            }

            if ((Count & RemainderFilter) != 0)
            {
                Data.Add(Current);
            }

            return true;
        }
        #endregion

        #region IEqualityComparer
        public bool Equals(BitCollection? Left, BitCollection? Right)
        {
            return Left == Right;
        }

        public int GetHashCode(BitCollection Collection)
        {
            return Collection.GetHashCode();
        }
        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<bool> GetEnumerator()
        {
            var BitCount = Length;

            for (var Index = 0; Index < BitCount; Index++)
            {
                yield return Bits[Index >> BinaryGroupSize].GetBit(Index & RemainderFilter);
            }
        }
        #endregion
    }
}