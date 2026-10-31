namespace Zion
{
    public sealed class ArenaBuffer<T> : ArenaCollection<T>
    {
        public int Count
        {
            get;
            private set
            {
                field = value;
                UpdateLimit(value);
            }
        }

        public int Capacity => GetLength();


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
            int Index = Count;
            EnsureCapacity(Count + 1);
            this[Index] = Item;
            Count++;
        }

        public void AddRange(IEnumerable<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);

            int Count = this.Count;

            if (Items.TryGetNonEnumeratedCount(out int ItemsCount))
            {
                int TotalCount = Count + ItemsCount;
                EnsureCapacity(TotalCount);
                UseSpan
                (
                    Count, ItemsCount, Items, static (Span, Other) =>
                    {
                        int Index = 0;

                        foreach (T Item in Other)
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
            int TotalCount = Count + ItemsCount;

            EnsureCapacity(TotalCount);
            UseSpan
            (
                Count, ItemsCount, Items,
                static (Span, Other) =>
                {
                    for (int i = 0; i < Other.Length; i++)
                    {
                        Span[i] = Other[i];
                    }
                }
            );
            Count = TotalCount;
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

        public void RemoveAt(int Index)
        {
            ThrowIfWithout(Index);

            Copy(Index + 1, Index, Count - Index - 1);
            Count--;
        }

        public void RemoveRange(int Index, int Count)
        {
            ThrowIfWithout(Index);
            ThrowIfWithout(Index + Count - 1);

            Copy(Index + Count, Index, this.Count - Count - Index);
            this.Count -= Count;
        }

        public void Clear()
        {
            ClearAll();
            Count = 0;
        }


        public T First()
        {
            return this[0];
        }

        public T Last()
        {
            return this[Count - 1];
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan().IndexOf(Item, Comparer);
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return AsSpan().Contains(Item, Comparer);
        }


        public void Insert(int Index, T Item)
        {
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Count);

            EnsureCapacity(Count + 1);
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
                    (Self: this, Offset: TotalCount),
                    static (TailSpan, State) => TailSpan.CopyTo(State.Self.AsSpan(State.Offset, TailSpan.Length))
                );

                Count = TotalCount + TailLength;
            }
        }

        public void Reverse()
        {
            AsSpan().Reverse();
        }

        public void Sort()
        {
            AsSpan().Sort();
        }


        public T[] ToArray()
        {
            return AsSpan().ToArray();
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

        
        public void Expand(int Additional)
        {
            Extend(Count + Additional);
        }

        public void EnsureCapacity(int Capacity)
        {
            Extend(Capacity);
        }


        public void CopyTo(T[] Array, int ArrayIndex)
        {
            CopyTo(Array.AsSpan(ArrayIndex));
        }

        public void CopyTo(Span<T> Destination)
        {
            AsSpan().CopyTo(Destination);
        }


        public ArenaItemEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(0, Count);
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

                Expand(LocalIndex);
                UseSpan
                (
                    TotalIndex, LocalIndex, Buffer.AsSpan(0, LocalIndex),
                    static (Span, Other) =>
                    {
                        for (int i = 0; i < Other.Length; i++)
                        {
                            Span[i] = Other[i];
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