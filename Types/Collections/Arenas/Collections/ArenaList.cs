using System.Runtime.InteropServices;

namespace Zion
{
    public sealed class ArenaList<T> : ArenaCollection<T>, IList<T>
    {
        public int Count
        {
            get;
            private set
            {
                Modify();
                field = value;
            }
        }

        public bool IsReadOnly => false;


        public new T this[int Index]
        {
            get => base[Index];
            set => base[Index] = value;
        }

        public new T this[Index Index]
        {
            get => base[Index];
            set => base[Index] = value;
        }


        protected override int GetSpanLimit() => Count;

        protected override IEnumerator<int> GetIndexEnumerator()
        {
            int Count = this.Count;
            for (int i = 0; i < Count; i++)
            {
                yield return i;
            }
        }


        public void Add(T Item)
        {
            int Index = Count++;
            Expand(Count);
            this[Index] = Item;
        }

        public void AddRange(IEnumerable<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);

            int Count = this.Count;

            if (Items.TryGetNonEnumeratedCount(out int ItemsCount))
            {
                int TotalCount = Count + ItemsCount;
                Expand(TotalCount);
                UseSpan
                (
                    Count, ItemsCount, Span =>
                    {
                        int Index = 0;

                        foreach (T Item in Items)
                        {
                            Span[Index++] = Item;
                        }
                    }
                );
                this.Count = TotalCount;
            }
            else
            {
                InsertItems(Count, Items, out int NewCount);
                this.Count = NewCount;
            }
        }

        public void AddRange(ReadOnlySpan<T> Items)
        {
            int ItemsCount = Items.Length;
            int TotalCount = Count + Items.Length;

            Expand(TotalCount);
            UseSpan
            (
                Count, ItemsCount, Items,
                (Span, Other) =>
                {
                    for (int i = 0; i < ItemsCount; i++)
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
            Move(Index, Index + 1, Count - Index);
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
                Expand(TotalCount);

                if (Index < Count)
                {
                    Move(Index, Index + ItemsCount, Count - Index);
                }

                UseSpan
                (
                    Index, ItemsCount, Span =>
                    {
                        int SpanIndex = 0;
                        foreach (T Item in Items)
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
                using var Tail = Source.GetArray(TailLength);

                if (TailLength > 0)
                {
                    Tail.UseSpan(Span => CopyTo(Index, Count - Index, Span));
                }

                InsertItems(Index, Items, out int TotalCount);

                Expand(TotalCount + TailLength);
                UseSpan(TotalCount, TailLength, Tail.CopyTo);

                Count = TotalCount + TailLength;
            }
        }


        public int IndexOf(T Item)
        {
            return IndexOf(Item, null);
        }

        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Count, Span => Span.IndexOf(Item, Comparer));
        }

        public int IndexOf(T Item, int Index, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Index, Count, Span => Span.IndexOf(Item, Comparer));
        }

        public int IndexOf(T Item, int Index, int Count, IEqualityComparer<T>? Comparer = null)
        {
            ThrowIfWithout(Index, Count);
            return UseReadOnlySpan(Index, Count, Span => Span.IndexOf(Item, Comparer));
        }


        public int FindIndex(Predicate<T> Match)
        {
            return FindIndex(Count, Match);
        }

        public int FindIndex(int Index, Predicate<T> Match)
        {
            return FindIndex(Index, Count - Index, Match);
        }

        public int FindIndex(int Index, int Count, Predicate<T> Match)
        {
            ThrowIfWithout(Index, Count);
            return UseReadOnlySpan
            (
                Count, Span =>
                {
                    int Count = Span.Length;
                    for (int i = 0; i < Count; i++)
                    {
                        if (Match(Span[i]))
                        {
                            return i;
                        }
                    }
                    return -1;
                }
            );
        }


        public T? Find(Predicate<T> Match)
        {
            return UseReadOnlySpan
            (
                Count, Span =>
                {
                    int Count = Span.Length;
                    for (int i = 0; i < Count; i++)
                    {
                        T Item = Span[i];
                        if (Match(Item))
                        {
                            return Item;
                        }
                    }
                    return default;
                }
            );
        }

        public T? FindLast(Predicate<T> Match)
        {
            return UseReadOnlySpan
            (
                Count, Span =>
                {
                    for (int i = Span.Length - 1; i >= 0; i--)
                    {
                        T Item = Span[i];
                        if (Match(Item))
                        {
                            return Item;
                        }
                    }
                    return default;
                }
            );
        }

        public List<T> FindAll(Predicate<T> Match)
        {
            return UseReadOnlySpan
            (
                Count, Span =>
                {
                    var Count = Span.Length;
                    var Result = new List<T>();
                    
                    for (int i = 0; i < Count; i++)
                    {
                        T Item = Span[i];
                        if (Match(Item))
                        {
                            Result.Add(Item);
                        }
                    }

                    return Result;
                }
            );
        }


        public int LastIndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Count, Span => Span.LastIndexOf(Item, Comparer));
        }

        public int LastIndexOf(T Item, int Index, IEqualityComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan(Index, Count, Span => Span.LastIndexOf(Item, Comparer));
        }

        public int LastIndexOf(T Item, int Index, int Count, IEqualityComparer<T>? Comparer = null)
        {
            ThrowIfWithout(Index, Count);
            return UseReadOnlySpan(Index, Count, Span => Span.LastIndexOf(Item, Comparer));
        }


        public int FindLastIndex(Predicate<T> Match)
        {
            return FindLastIndex(0, Match);
        }

        public int FindLastIndex(int Index, Predicate<T> Match)
        {
            return FindLastIndex(Index, Count - Index, Match);
        }

        public int FindLastIndex(int Index, int Count, Predicate<T> Match)
        {
            ThrowIfWithout(Index, Count);
            return UseReadOnlySpan
            (
                Count, Span =>
                {
                    for (int i = Span.Length; i >= 0; i--)
                    {
                        if (Match(Span[i]))
                        {
                            return i;
                        }
                    }
                    return -1;
                }
            );
        }


        public bool Contains(T Item)
        {
            return Contains(Item, null);
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return IndexOf(Item, Comparer) != -1;
        }

        public bool Exists(Predicate<T> Match)
        {
            return FindIndex(Match) != -1;
        }


        public void CopyTo(T[] Array)
        {
            CopyTo(Array);
        }

        public void CopyTo(T[] Array, int ArrayIndex)
        {
            CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(int Index, T[] Array, int ArrayIndex, int Count)
        {
            ThrowIfWithout(Index, Count);
            UseReadOnlySpan
            (
                Index, Count, Span =>
                {
                    Span.CopyTo(Array.AsSpan(ArrayIndex, Count));
                }
            );
        }


        public void RemoveAt(int Index)
        {
            ThrowIfWithout(Index);
            Move(Index + 1, Index, Count - Index - 1);
            Count --;
        }

        public void RemoveRange(int Index, int Count)
        {
            ThrowIfWithout(Index, Count);
            Move(Index + Count, Index, Count);
            this.Count -= Count;
        }

        public bool Remove(T Item)
        {
            int Index = IndexOf(Item);
            
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

            int WriteIndex = 0;
            int RemoveCount = 0;

            for (int readIndex = 0; readIndex < Count; readIndex++)
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
            Count = 0;
        }


        public void Reverse(int Index, int Count)
        {
            UseSpan(Index, Count, static Span => Span.Reverse());
        }

        public void Reverse()
        {
            UseSpan(Count, static Span => Span.Reverse());
        }


        public void Sort()
        {
            UseSpan(Count, static Span => Span.Sort());
        }

        public void Sort(IComparer<T>? Comparer)
        {
            UseSpan(Count, Span => Span.Sort(Comparer));
        }

        public void Sort(Comparison<T> Comparison)
        {
            UseSpan(Count, Span => Span.Sort(Comparison));
        }

        public void Sort(int Index, int Count, IComparer<T>? Comparer = null)
        {
            ThrowIfWithout(Index, Count);
            UseSpan(Index, Count, Span => Span.Sort(Comparer));
        }


        public int BinarySearch(T Item, IComparer<T>? Comparer = null)
        {
            return UseReadOnlySpan
            (
                Count, Span => Span.BinarySearch(Item, Comparer ?? Comparer<T>.Default)
            );
        }

        public int BinarySearch(int Index, int Count, T Item, IComparer<T> Comparer)
        {
            ThrowIfWithout(Index, Count);
            return UseReadOnlySpan
            (
                Index, Count, Span => Span.BinarySearch(Item, Comparer ?? Comparer<T>.Default)
            );
        }


        public List<T> GetRange(int Index, int Count)
        {
            ThrowIfWithout(Index, Count);
            return UseReadOnlySpan
            (
                Index, Count, Span =>
                {
                    var List = new List<T>(Span.Length);
                    Span.CopyTo(CollectionsMarshal.AsSpan(List));
                    return List;
                }
            );
        }


        public void ForEach(Action<T> Action)
        {
            UseReadOnlySpan
            (
                Span =>
                {
                    int Count = Span.Length;
                    for (int i = 0; i < Count; i++)
                    {
                        Action(Span[i]);
                    }
                }
            );
        }

        public bool TrueForAll(Predicate<T> Match)
        {
            return UseReadOnlySpan
            (
                Span =>
                {
                    int Count = Span.Length;
                    for (int i = 0; i < Count; i++)
                    {
                        if (!Match(Span[i]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            );
        }

        public List<TOut> ConvertAll<TOut>(Converter<T, TOut> Converter)
        {
            return UseReadOnlySpan
            (
                Count, Span =>
                {
                    int Count = Span.Length;

                    var List = new List<TOut>(Count);
                    var Destination = CollectionsMarshal.AsSpan(List);

                    for (int i = 0; i < Count; i++)
                    {
                        Destination[i] = Converter(Span[i]);
                    }

                    return List;
                }
            );
        }


        public T[] ToArray()
        {
            return UseReadOnlySpan(Count, static Span => Span.ToArray());
        }

        public List<T> ToList()
        {
            return UseReadOnlySpan
            (
                Count,
                Span =>
                {
                    var List = new List<T>(Span.Length);
                    Span.CopyTo(CollectionsMarshal.AsSpan(List));
                    return List;
                }
            );
        }


        public new void UseSpan(Action<Span<T>> Action)
        {
            base.UseSpan(Action);
        }

        public new void UseSpan(int Count, Action<Span<T>> Action)
        {
            base.UseSpan(Count, Action);
        }

        public new void UseSpan(int Start, int Count, Action<Span<T>> Action)
        {
            base.UseSpan(Start, Count, Action);
        }


        public new void UseSpan<I>(I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Other, Action);
        }

        public new void UseSpan<I>(int Count, I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Count, Other, Action);
        }

        public new void UseSpan<I>(int Start, int Count, I Other, Action<Span<T>, I> Action) where I : allows ref struct
        {
            base.UseSpan(Start, Count, Other, Action);
        }


        public new R UseSpan<R>(Func<Span<T>, R> Function)
        {
            return base.UseSpan(Function);
        }

        public new R UseSpan<R>(int Count, Func<Span<T>, R> Function)
        {
            return base.UseSpan(Count, Function);
        }

        public new R UseSpan<R>(int Start, int Count, Func<Span<T>, R> Function)
        {
            return base.UseSpan(Start, Count, Function);
        }


        public new R UseSpan<I, R>(I Other, Func<Span<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseSpan(Other, Action);
        }

        public new R UseSpan<I, R>(int Count, I Other, Func<Span<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseSpan(Count, Other, Action);
        }

        public new R UseSpan<I, R>(int Start, int Count, I Other, Func<Span<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseSpan(Start, Count, Other, Action);
        }


        public new void UseReadOnlySpan(Action<ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Action);
        }

        public new void UseReadOnlySpan(int Count, Action<ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Count, Action);
        }

        public new void UseReadOnlySpan(int Start, int Count, Action<ReadOnlySpan<T>> Action)
        {
            base.UseReadOnlySpan(Start, Count, Action);
        }


        public new void UseReadOnlySpan<I>(I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            base.UseReadOnlySpan(Other, Action);
        }

        public new void UseReadOnlySpan<I>(int Count, I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            base.UseReadOnlySpan(Count, Other, Action);
        }

        public new void UseReadOnlySpan<I>(int Start, int Count, I Other, Action<ReadOnlySpan<T>, I> Action) where I : allows ref struct
        {
            base.UseReadOnlySpan(Start, Count, Other, Action);
        }


        public new R UseReadOnlySpan<R>(Func<ReadOnlySpan<T>, R> Function)
        {
            return base.UseReadOnlySpan(Function);
        }

        public new R UseReadOnlySpan<R>(int Count, Func<ReadOnlySpan<T>, R> Function)
        {
            return base.UseReadOnlySpan(Count, Function);
        }

        public new R UseReadOnlySpan<R>(int Start, int Count, Func<ReadOnlySpan<T>, R> Function)
        {
            return base.UseReadOnlySpan(Start, Count, Function);
        }


        public new R UseReadOnlySpan<I, R>(I Other, Func<ReadOnlySpan<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseReadOnlySpan(Other, Action);
        }

        public new R UseReadOnlySpan<I, R>(int Count, I Other, Func<ReadOnlySpan<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseReadOnlySpan(Count, Other, Action);
        }

        public new R UseReadOnlySpan<I, R>(int Start, int Count, I Other, Func<ReadOnlySpan<T>, I, R> Action) where I : allows ref struct
        {
            return base.UseReadOnlySpan(Start, Count, Other, Action);
        }


        public new void Expand(int Additional)
        {
            base.Expand(Additional);
        }

        public new void EnsureCapacity(int Capacity)
        {
            base.EnsureCapacity(Capacity);
        }


        private void ThrowIfWithout(int Index)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Index, Count);
        }

        private void ThrowIfWithout(int Index, int Count)
        {
            int TotalCount = this.Count;
            ArgumentOutOfRangeException.ThrowIfWithout(Index, TotalCount);
            ArgumentOutOfRangeException.ThrowIfWithout(Index + Count, TotalCount);
        }


        private void InsertItems(int Index, IEnumerable<T> Items, out int Count)
        {
            const int BufferSize = 0x20;

            T[] Buffer = new T[BufferSize];
            int LocalIndex = 0;
            int TotalIndex = Index;

            void Flush()
            {
                if (LocalIndex == 0)
                {
                    return;
                }

                Expand(TotalIndex + LocalIndex);
                UseSpan
                (
                    TotalIndex, LocalIndex, Span =>
                    {
                        for (int i = 0; i < LocalIndex; i++)
                        {
                            Span[i] = Buffer[i];
                        }
                    }
                );

                TotalIndex += LocalIndex;
                LocalIndex = 0;
            }

            foreach (T Item in Items)
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