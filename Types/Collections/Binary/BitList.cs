using Zion.Serialization;

namespace Zion
{
    public sealed class BitList : BitCollection, IList<bool>, IBinarySerializable<BitList>, IParsable<BitList>
    {
        #region Properties
        private int _Count;
        public override int Count => _Count;

        public int Capacity => Data.Length << 3;

        public bool IsReadOnly => false;

        #endregion

        #region Constructors
        public BitList(int Count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Count);

            Data = new ulong[GetGroupCount(Count)];
            _Count = Count;
        }

        public BitList(IEnumerable<ulong> Data, int Count)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Count);

            var GroupCount = GetGroupCount(Count);
            var Array = Data.ToArray();

            if (Array.Length < GroupCount)
            {
                System.Array.Resize(ref Array, GroupCount);
            }

            this.Data = Array;
            _Count = Count;
        }

        private BitList(int Count, ulong[] Data)
        {
            this.Data = Data;
            _Count = Count;
        }

        #endregion

        #region IList
        public void Add(bool Item)
        {
            int Index = Count;
            EnsureCapacity(++_Count);
            if (Item)
            {
                int ByteIndex = Index >> 3;
                Data[ByteIndex] = Data[ByteIndex].SetBit(Index & 0b111, Item);
            }
        }

        public void Insert(int Index, bool Item)
        {
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Count);

            EnsureCapacity(++_Count);

            int LastByte = (Count - 1) >> 3;
            int InsertByte = Index >> 3;
            int InsertBit = Index & 0b111;

            bool Carry = Item;

            for (int ByteIndex = InsertByte; ByteIndex <= LastByte; ByteIndex++)
            {
                int StartBit = (ByteIndex == InsertByte) ? InsertBit : 0;
                int EndBit = (ByteIndex == LastByte) ? ((Count - 1) & RemainderFilter) : RemainderFilter;

                (Data[ByteIndex], Carry) = InsertBitShift(Data[ByteIndex], StartBit, EndBit, Carry);
            }
        }


        public void CopyTo(bool[] Array, int ArrayIndex)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(ArrayIndex, Array);
            ArgumentOutOfRangeException.ThrowIfBeyond(Count, Array.Length - ArrayIndex);

            foreach (bool Item in this)
            {
                Array[ArrayIndex++] = Item;
            }
        }

        public BitList Clone()
        {
            return new BitList(_Count, ZArray.Clone(Data));
        }


        public bool Remove(bool Item)
        {
            int Index = IndexOf(Item);
            
            if (Index != -1)
            {
                RemoveAt(Index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int Index)
        {
            if (Index < 0 || Index >= Count)
            {
                throw new ArgumentOutOfRangeException($"Index(={Index}) out of range [0..{Count})");
            }

            int LastByte = (Count - 1) >> BinaryGroupSize;
            int RemoveByte = Index >> BinaryGroupSize;
            int RemoveBit = Index & RemainderFilter;

            bool Carry = false;

            for (int ByteIndex = LastByte; ByteIndex >= RemoveByte; ByteIndex--)
            {
                int StartBit = (ByteIndex == RemoveByte) ? RemoveBit : 0;
                int EndBit = (ByteIndex == LastByte) ? ((Count - 1) & RemainderFilter) : RemainderFilter;

                (Data[ByteIndex], Carry) = RemoveBitShift(Data[ByteIndex], StartBit, EndBit, Carry);
            }

            _Count--;

            int ClearByte = Count >> 3;
            int ClearBit = Count & 0b111;
            Data[ClearByte] = Data[ClearByte].SetBit(ClearBit, false);
        }

        public void Clear()
        {
            _Count = 0;
        }

        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer)
        {
            Writer.Write(Count);
            Writer.BaseStream.Write(AsByteSpan());
        }

        public static BitList Read(BinaryReader Reader)
        {
            int Count = Reader.ReadInt32();
            int GroupCount = GetGroupCount(Count);
            ulong[] Buffer = new ulong[GroupCount];

            for (int i = 0; i < GroupCount; i++)
            {
                Buffer[i] = Reader.ReadUInt64();
            }

            return new BitList(Count, Buffer);
        }

        #endregion

        #region IParsable
        public static BitList Parse(string String, IFormatProvider? Provider)
        {
            var Pair = ParseBitCollection(String);
            return new BitList(Pair.Item1, Pair.Item2);
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

        #region PublicMethods
        public void EnsureCapacity(int Capacity)
        {
            if (Capacity <= this.Capacity)
            {
                return;
            }

            ulong[] NewData = new ulong[GetGroupCount(Capacity)];
            Array.Copy(Data, NewData, GetGroupCount(Count));
            Data = NewData;
        }

        public BitArray ToBitArray()
        {
            return new BitArray(Data, Count);
        }

        #endregion

        #region PrivateMethods
        private static (ulong, bool) InsertBitShift(ulong Value, int StartBit, int EndBit, bool Carry)
        {
            int ShiftedCount = EndBit - StartBit + 1;

            ulong Mask = ((1UL << ShiftedCount) - 1) << StartBit;
            ulong Shifted = ((Value & Mask) << 1);

            Value = (Value & ~Mask) | (Shifted & Mask);

            if (Carry)
            {
                Value |= (byte)(1 << StartBit);
            }

            bool NewCarry = (Value & (1UL << (EndBit + 1))) != 0;

            if (EndBit < 7)
            {
                Value &= ~(1UL << (EndBit + 1));
            }

            return (Value, NewCarry);
        }

        private static (ulong, bool) RemoveBitShift(ulong Value, int StartBit, int EndBit, bool Carry)
        {
            bool Removed = (Value & (1UL << StartBit)) != 0;

            int ShiftedCount = EndBit - StartBit;

            if (ShiftedCount > 0)
            {
                ulong Mask = ((1UL << ShiftedCount) - 1) << (StartBit + 1);
                ulong Shifted = (Value & Mask) >> 1;

                Value = (byte)((Value & ~Mask) | (Shifted & Mask));
                Value &= (byte)~(1 << EndBit);
            }

            if (Carry)
            {
                Value |= 1UL << EndBit;
            }
            else
            {
                Value &= ~1UL << EndBit;
            }

            return (Value, Removed);
        }

        #endregion
    }
}