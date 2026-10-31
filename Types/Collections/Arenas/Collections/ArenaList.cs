using System.Runtime.InteropServices;

namespace Zion
{
    public sealed class ArenaList<T> : ArenaCollection<T>
    {
        public int Count
        {
            get;
            private set
            {
                UpdateLimit(value);
                field = value;
            }
        }

        public bool IsReadOnly => false;


        public T this[int Index]
        {
            get => GetValueAt(Index);
            set => SetValueAt(Index, value);
        }

        public T this[Index Index]
        {
            get => GetValueAt(Index);
            set => SetValueAt(Index, value);
        }


        public void Add(T Item)
        {
            var Index = Count++;
            Expand(Count);
            this[Index] = Item;
        }

        public void AddRange(IEnumerable<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);

            var Count = this.Count;

            if (Items.TryGetNonEnumeratedCount(out var ItemsCount))
            {
                var TotalCount = Count + ItemsCount;
                Expand(TotalCount);
                UseSpan
                (
                    Count, ItemsCount, Items, static (Span, Other) =>
                    {
                        var Index = 0;

                        foreach (var Item in Other)
                        {
                            Span[Index++] = Item;
                        }
                    }
                );
                this.Count = TotalCount;
            }
            else
            {
                InsertItems(Count, Items, out var NewCount);
                this.Count = NewCount;
            }
        }

        public void AddRange(ReadOnlySpan<T> Items)
        {
            var ItemsCount = Items.Length;
            var TotalCount = Count + Items.Length;

            Expand(TotalCount);
            UseSpan
            (
                Count, ItemsCount, Items,
                static (Span, Other) =>
                {
                    for (var i = 0; i < Other.Length; i++)
                    {
                        Span[i] = Other[i];
                    }
                }
            );
            Count = TotalCount;
        }

        public void Insert(int Index, T Item)
        {
            ThrowIfWithout(Index);
            Copy(Index, Index + 1, Count - Index);
            this[Index] = Item;
            Count++;
        }

        public void InsertRange(int Index, IEnumerable<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Count);

            if (Items.TryGetNonEnumeratedCount(out int ItemsCount))
            {
                if (ItemsCount == 0)
                {
                    return;
                }

                int TotalCount = Count + ItemsCount;
                EnsureCapacity(TotalCount);

                if (Index < Count)
                {
                    Copy(Index, Index + ItemsCount, Count - Index);
                }

                UseSpan
                (
                    Index, ItemsCount, Items, static (Span, Other) =>
                    {
                        int SpanIndex = 0;
                        foreach (T Item in Other)
                        {
                            Span[SpanIndex++] = Item;
                        }
                    }
                );

                Count = TotalCount;
            }
            else
            {
                int TailLength = Count - Index;
                using var Tail = Allocate<ArenaBuffer<T>>(TailLength);

                if (TailLength > 0)
                {
                    Tail.UseSpan
                    (
                        AsSpan(Index, TailLength),
                        static (TailSpan, Source) => Source.CopyTo(TailSpan)
                    );
                }

                InsertItems(Index, Items, out int TotalCount);
                EnsureCapacity(TotalCount + TailLength);

                Tail.UseSpan
                (
                    AsSpan(TotalCount, Tail.Count),
                    static (TailSpan, Destination) => TailSpan.CopyTo(Destination)
                );

                Count = TotalCount + TailLength;
            }
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan().IndexOf(Item, Comparer);
        }

        public int IndexOf(T Item, int Index, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan(Index).IndexOf(Item, Comparer);
        }

        public int IndexOf(T Item, int Index, int Count, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan(Index, Count).IndexOf(Item, Comparer);
        }


        public int FindIndex(Predicate<T> Match)
        {
            return AsSpan().FindIndex(Match);
        }

        public int FindIndex(int Index, Predicate<T> Match)
        {
            return AsSpan(Index).FindIndex(Match);
        }

        public int FindIndex(int Index, int Count, Predicate<T> Match)
        {
            return AsSpan(Index, Count).FindIndex(Match);
        }


        public T? Find(Predicate<T> Match)
        {
            return AsSpan().Find(Match);
        }

        public T? FindLast(Predicate<T> Match)
        {
            return AsSpan().FindLast(Match);
        }

        public List<T> FindAll(Predicate<T> Match)
        {
            return AsSpan().FindAll(Match);
        }


        public int LastIndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan().LastIndexOf(Item, Comparer);
        }

        public int LastIndexOf(T Item, int Index, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan(Index).LastIndexOf(Item, Comparer);
        }

        public int LastIndexOf(T Item, int Index, int Count, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan(Index, Count).LastIndexOf(Item, Comparer);
        }


        public int FindLastIndex(Predicate<T> Match)
        {
            return AsSpan().FindLastIndex(Match);
        }

        public int FindLastIndex(int Index, Predicate<T> Match)
        {
            return AsSpan(Index).FindLastIndex(Match);
        }

        public int FindLastIndex(int Index, int Count, Predicate<T> Match)
        {
            return AsSpan(Index, Count).FindLastIndex(Match);
        }


        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan().Contains(Item, Comparer);
        }

        public bool Exists(Predicate<T> Match)
        {
            return AsSpan().Exists(Match);
        }


        public void CopyTo(T[] Array)
        {
            AsSpan().CopyTo(Array);
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            AsSpan().CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(int Index, T[] Array, int ArrayIndex, int Count)
        {
            AsSpan(Index).CopyTo(Array.AsSpan(ArrayIndex, Count));
        }


        public void RemoveAt(int Index)
        {
            ThrowIfWithout(Index);
            Copy(Index + 1, Index, Count - Index - 1);
            Count--;
        }

        public void RemoveRange(int Index, int Count)
        {
            ThrowIfWithout(Index, Count);
            Copy(Index + Count, Index, this.Count - Index - Count);
            this.Count -= Count;
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

        public int RemoveAll(Predicate<T> Match)
        {
            ArgumentNullException.ThrowIfNull(Match);

            var WriteIndex = 0;
            var RemoveCount = 0;

            for (var readIndex = 0; readIndex < Count; readIndex++)
            {
                if (!Match(this[readIndex]))
                {
                    if (WriteIndex != readIndex)
                    {
                        this[WriteIndex] = this[readIndex];
                    }
                    WriteIndex++;
                }
                else
                {
                    RemoveCount++;
                }
            }

            if (RemoveCount > 0)
            {
                RemoveRange(WriteIndex, RemoveCount);
                Count = WriteIndex;
            }

            return RemoveCount;
        }

        public void Clear()
        {
            ClearAll();
            Count = 0;
        }


        public void Reverse()
        {
            AsSpan().Reverse();
        }

        public void Reverse(int Index, int Count)
        {
            AsSpan(Index, Count).Reverse();
        }

        public void Sort(IComparer<T>? Comparer = null)
        {
            AsSpan().Sort(Comparer);
        }

        public void Sort(int Index, int Count, IComparer<T>? Comparer = null)
        {
            AsSpan(Index, Count).Sort(Comparer);
        }


        public int BinarySearch(T Item, IComparer<T>? Comparer = null)
        {
            return AsSpan().BinarySearch(Item, Comparer);
        }

        public int BinarySearch(int Index, int Count, T Item, IComparer<T>? Comparer = null)
        {
            return AsSpan(Index, Count).BinarySearch(Item, Comparer);
        }


        public List<T> GetRange(int Index, int Count)
        {
            return AsSpan(Index, Count).ToList();
        }


        public void ForEach(Action<T> Action)
        {
            AsSpan().ForEach(Action);
        }

        public bool TrueForAll(Predicate<T> Match)
        {
            return AsSpan().TrueForAll(Match);
        }

        public List<TOut> ConvertAll<TOut>(Converter<T, TOut> Converter)
        {
            var Length = Count;
            var Result = new List<TOut>(Length);
            var Source = AsSpan();
            
            CollectionsMarshal.SetCount(Result, Length);
            var Destination = CollectionsMarshal.AsSpan(Result);

            for (int i = 0; i < Length; i++)
            {
                Destination[i] = Converter(Source[i]);
            }
            
            return Result;
        }


        public T[] ToArray()
        {
            return AsSpan().ToArray();
        }

        public List<T> ToList()
        {
            return AsSpan().ToList();
        }


        public new void UseSpan(Action<ArenaSpan<T>> Action)
        {
            base.UseSpan(Action);
        }

        public new void UseSpan(int Start, Action<ArenaSpan<T>> Action)
        {
            base.UseSpan(Start, Action);
        }

        public new void UseSpan(int Start, int Count, Action<ArenaSpan<T>> Action)
        {
            base.UseSpan(Start, Count, Action);
        }


        public new void UseSpan<I>(I Other, Action<ArenaSpan<T>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Other, Action);
        }

        public new void UseSpan<I>(int Start, I Other, Action<ArenaSpan<T>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Start, Other, Action);
        }

        public new void UseSpan<I>(int Start, int Count, I Other, Action<ArenaSpan<T>, I> Action)
            where I : allows ref struct
        {
            base.UseSpan(Start, Count, Other, Action);
        }


        public new R UseSpan<R>(Func<ArenaSpan<T>, R> Function)
        {
            return base.UseSpan(Function);
        }

        public new R UseSpan<R>(int Start, Func<ArenaSpan<T>, R> Function)
        {
            return base.UseSpan(Start, Function);
        }

        public new R UseSpan<R>(int Start, int Count, Func<ArenaSpan<T>, R> Function)
        {
            return base.UseSpan(Start, Count, Function);
        }


        public new R UseSpan<I, R>(I Other, Func<ArenaSpan<T>, I, R> Function)
            where I : allows ref struct where R : allows ref struct
        {
            return base.UseSpan(Other, Function);
        }

        public new R UseSpan<I, R>(int Start, I Other, Func<ArenaSpan<T>, I, R> Function)
            where I : allows ref struct where R : allows ref struct
        {
            return base.UseSpan(Start, Other, Function);
        }

        public new R UseSpan<I, R>(int Start, int Count, I Other, Func<ArenaSpan<T>, I, R> Function)
            where I : allows ref struct where R : allows ref struct
        {
            return base.UseSpan(Start, Count, Other, Function);
        }


        public void Expand(int Capacity)
        {
            Extend(Count + Capacity);
        }

        public void EnsureCapacity(int Capacity)
        {
            Extend(Capacity);
        }


        public ArenaItemEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(0, Count);
        }


        private void InsertItems(int Index, IEnumerable<T> Items, out int Count)
        {
            const int BufferSize = 0x20;

            var Buffer = new T[BufferSize];
            var LocalIndex = 0;
            var TotalIndex = Index;

            void Flush()
            {
                if (LocalIndex == 0)
                {
                    return;
                }

                Expand(TotalIndex + LocalIndex);
                UseSpan
                (
                    TotalIndex, LocalIndex, Buffer.AsSpan(0, LocalIndex),
                    static (Span, Other) =>
                    {
                        for (var i = 0; i < Other.Length; i++)
                        {
                            Span[i] = Other[i];
                        }
                    }
                );

                TotalIndex += LocalIndex;
                LocalIndex = 0;
            }

            foreach (var Item in Items)
            {
                Buffer[LocalIndex++] = Item;

                if (LocalIndex >= BufferSize)
                {
                    Flush();
                }
            }

            Flush();
            Count = TotalIndex;
        }
    }
}