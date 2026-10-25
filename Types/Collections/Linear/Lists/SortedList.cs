using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Zion.Serialization;

namespace Zion
{
    public class SortedList<T> : IList<T>, IBinarySerializable<SortedList<T>, T>
    {
        #region Data
        private readonly bool         IsReference = RuntimeHelpers.IsReferenceOrContainsReferences<T>();
        private readonly IComparer<T> Comparer;

        private T[] Data;

        public int Count { get; private set; }

        public int  Capacity   => Data.Length;
        public bool IsReadOnly => false;
        #endregion

        #region Constructors
        public SortedList() : this(16) { }

        public SortedList(IComparer<T> Comparer) : this(16, Comparer) { }

        public SortedList(int Capacity) : this(Capacity, Comparer<T>.Default) { }

        public SortedList(int Capacity, IComparer<T> Comparer)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Capacity);
            this.Comparer = Comparer ?? Comparer<T>.Default;
            Data = new T[Capacity];
        }

        private SortedList(T[] Data, int Count)
        {
            Comparer = Comparer<T>.Default;
            this.Data = Data;
            this.Count = Count;
            Update();
        }
        #endregion

        #region Indexers
        public T this[int Index]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);
                return Data[Index];
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);

                var ValidLeft = Index == 0 || Compare(value, Data[Index - 1]) >= 0;
                var ValidRight = Index == Count - 1 || Compare(value, Data[Index + 1]) <= 0;

                if (ValidLeft && ValidRight)
                {
                    Data[Index] = value;
                }
                else
                {
                    RemoveAt(Index);
                    Add(value);
                }
            }
        }

        public T this[Index Index]
        {
            get => this[Index.GetOffset(Count)];
            set => this[Index.GetOffset(Count)] = value;
        }

        public T[] this[Range Range]
        {
            get
            {
                ArgumentOutOfRangeException.ThrowIfWithout(Range.Start.GetOffset(Count), Count);
                ArgumentOutOfRangeException.ThrowIfWithout(Range.End.GetOffset(Count), Count);
                return Data[Range];
            }
        }
        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            return StringFormatter.ToString(this);
        }
        #endregion

        #region IList
        public void Update()
        {
            Array.Sort(Data, 0, Count);
        }


        public void Add(T Item)
        {
            InsertAt(Item, GetInsertIndex(Item));
        }

        public void AddRange(ICollection<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);

            EnsureCapacity(Count + Items.Count);

            foreach (var Item in Items)
            {
                InsertAt(Item, GetInsertIndex(Item));
            }
        }

        public void Insert(int TargetIndex, T Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            ArgumentOutOfRangeException.ThrowIfBeyond(TargetIndex, Count);

            if (TargetIndex < 0 || TargetIndex > Count)
            {
                Add(Item);
                return;
            }

            var ValidLeft = TargetIndex == 0 || Compare(Item, Data[TargetIndex - 1]) >= 0;
            var ValidRight = TargetIndex == Count || Compare(Item, Data[TargetIndex]) <= 0;

            if (ValidLeft && ValidRight)
            {
                InsertAt(Item, TargetIndex);
            }
            else
            {
                Add(Item);
            }
        }


        public int IndexOf(T Item)
        {
            if (Count == 0)
            {
                return -1;
            }

            var Min = 0;
            var Max = Count - 1;

            while (Min <= Max)
            {
                var Target = Min + ((Max - Min) >> 1);
                var Comparing = Compare(Item, Data[Target]);

                if (Comparing == 0)
                {
                    return Target;
                }
                if (Int32.IsNegative(Comparing))
                {
                    Max = Target - 1;
                }
                else
                {
                    Min = Target + 1;
                }
            }

            return -1;
        }

        public bool Contains(T Item)
        {
            return IndexOf(Item) != -1;
        }

        public bool TryFindFirstAtLeast(T Target, out int Index)
        {
            Index = FindFirstAtLeast(Target);
            return Index != -1;
        }

        public int FindFirstAtLeast(T Target)
        {
            var Data = this.Data;
            var Comparer = this.Comparer;

            if (Count == 0 || Comparer.Compare(Data[Count - 1], Target) < 0)
            {
                return -1;
            }
            
            var Min = 0;
            var Max = Count;

            while (Min < Max)
            {
                var Middle = Min + ((Max - Min) >> 1);

                if (Comparer.Compare(Data[Middle], Target) < 0)
                {
                    Min = Middle + 1;
                }
                else
                {
                    Max = Middle;
                }
            }

            return Min;
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            ArgumentNullException.ThrowIfNull(Array);
            ArgumentOutOfRangeException.ThrowIfNegative(ArrayIndex);
            ArgumentException.ThrowIf(Array.Length - ArrayIndex < Count, "Target array is too small");
            System.Array.Copy(Data, 0, Array, ArrayIndex, Count);
        }


        public bool Remove(T Item)
        {
            var Index = IndexOf(Item);
            if (Index == -1)
            {
                return false;
            }
            RemoveAt(Index);
            return true;
        }

        public void RemoveAt(int Index)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);

            for (var i = Index; i < Count - 1; i++)
            {
                Data[i] = Data[i + 1];
            }

            Count--;
            if (IsReference)
            {
                Data[Count] = default!;
            }
        }

        public void RemoveRange(int Start, int Count)
        {
            ArgumentException.ThrowIf(Start < 0 || Start > this.Count,
                $"Start(={Start}) out of range [0..{this.Count})");
            ArgumentException.ThrowIf(Count < 0 || Start + Count > this.Count,
                $"Count(={Count}) out of range [0..{this.Count})");

            if (Count == 0)
            {
                return;
            }

            var End = Start + Count;

            foreach (var i in ZEnumerable.Range(this.Count - End))
            {
                Data[Start + i] = Data[End + i];
            }

            this.Count -= Count;

            if (IsReference)
            {
                Array.Clear(Data, this.Count, Count);
            }
        }

        public void Clear()
        {
            if (Count == 0)
            {
                return;
            }

            if (IsReference)
            {
                Array.Clear(Data, 0, Count);
            }

            Count = 0;
        }


        public void EnsureCapacity(int Capacity)
        {
            if (Capacity <= Data.Length)
            {
                return;
            }

            var NewCapacity = Math.Max(Data.Length * 2, Capacity);
            Array.Resize(ref Data, NewCapacity);
        }


        public T[] ToArray()
        {
            var Result = new T[Count];
            Array.Copy(Data, Result, Count);
            return Result;
        }

        public List<T> ToList()
        {
            var Result = new List<T>(Count);
            Data.AsSpan(0, Count).CopyTo(CollectionsMarshal.AsSpan(Result));
            return Result;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return Data[i];
            }
        }
        #endregion

        #region BinarySerializable
        public void Write(BinaryWriter Writer, Action<T> Write)
        {
            Writer.WriteCollection(this);
        }

        public static SortedList<T> Read(BinaryReader Reader, Func<T> Read)
        {
            var Count = Reader.ReadInt32();
            var Index = 0;

            var Result = new SortedList<T>(Count);

            while (Index < Count)
            {
                Result.Insert(Index, Read());
                Index++;
            }

            return Result;
        }
        #endregion

        #region PrivateMethods
        private int Compare(T A, T B)
        {
            return Comparer.Compare(A, B);
        }

        private int GetInsertIndex(T Item)
        {
            if (Count == 0)
            {
                return 0;
            }

            var Min = 0;
            var Max = Count - 1;

            while (Min <= Max)
            {
                var Target = Min + ((Max - Min) >> 1);
                var Comparing = Compare(Item, Data[Target]);

                if (Comparing == 0)
                {
                    return Target;
                }

                if (Comparing < 0)
                {
                    Max = Target - 1;
                }
                else
                {
                    Min = Target + 1;
                }
            }

            return Min;
        }

        private void InsertAt(int Index, T Item)
        {
            if (Count == Data.Length)
            {
                EnsureCapacity(Count + 1);
            }

            for (var i = Count; i > Index; i--)
            {
                Data[i] = Data[i - 1];
            }

            Data[Index] = Item;
            Count++;
        }
        #endregion
    }
}