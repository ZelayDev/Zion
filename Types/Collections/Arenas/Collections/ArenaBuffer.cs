namespace Zion
{
    public sealed class ArenaBuffer<T> : ArenaCollection<T>
    {
        //DO: Resize logic
        public int Length => GetLength();


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
            int Index = Length;
            EnsureCapacity(++Length);
            this[Index] = Item;
        }

        public void AddRange(IEnumerable<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);

            int Count = this.Length;

            if (Items.TryGetNonEnumeratedCount(out int ItemsCount))
            {
                int TotalCount = Count + ItemsCount;
                Extend(TotalCount);
                UseSpan
                (
                    Count, ItemsCount, static (Span, Items) =>
                    {
                        int Index = 0;

                        foreach (T Item in Items)
                        {
                            Span[Index++] = Item;
                        }
                    }, Items
                );
                this.Length = TotalCount;
            }
            else
            {
                InsertItems(Count, Items, out int NewCount);
                this.Length = NewCount;
            }
        }

        public void AddRange(ReadOnlySpan<T> Items)
        {
            int ItemsCount = Items.Length;
            int TotalCount = Length + Items.Length;

            Expand(TotalCount);
            UseSpan
            (
                Length, ItemsCount, Items,
                static (Span, Other) =>
                {
                    for (int i = 0; i < Other.Length; i++)
                    {
                        Span[i] = Other[i];
                    }
                }
            );
            Length = TotalCount;
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

            Move(Index + 1, Index, Length - Index - 1);
            Length--;
        }

        public void RemoveRange(int Index, int Count)
        {
            ThrowIfWithout(Index);
            ThrowIfWithout(Index + Count - 1);

            Move(Index + Count, Index, this.Length - Count - Index);
            this.Length -= Count;
        }

        public void Clear()
        {
            ClearAll();
        }


        public T First()
        {
            return this[0];
        }

        public T Last()
        {
            return this[Length - 1];
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseSpan(Length, static (Span, State) => Span.IndexOf(State.Item, State.Comparer), (Item, Comparer));
        }

        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            return UseSpan(Length, static (Span, State) => Span.Contains(State.Item, State.Comparer), (Item, Comparer));
        }


        public void Insert(int Index, T Item)
        {
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Length);

            Move(Index, Index + 1, Length - Index);
            this[Index] = Item;
            Length++;
        }

        public void InsertRange(int Index, IEnumerable<T> Items)
        {
            ArgumentNullException.ThrowIfNull(Items);
            ArgumentOutOfRangeException.ThrowIfBeyond(Index, Length);

            if (Items.TryGetNonEnumeratedCount(out int ItemsCount))
            {
                if (ItemsCount == 0)
                {
                    return;
                }

                int TotalCount = Length + ItemsCount;
                Extend(TotalCount);

                if (Index < Length)
                {
                    Move(Index, Index + ItemsCount, Length - Index);
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

                Length = TotalCount;
            }
            else
            {
                int TailLength = Length - Index;
                using var Tail = Source.GetBuffer(TailLength);

                if (TailLength > 0)
                {
                    Tail.UseSpan(Span => CopyTo(Index, Length - Index, Span));
                }

                InsertItems(Index, Items, out int TotalCount);

                Extend(TotalCount + TailLength);
                UseSpan(TotalCount, TailLength, Tail.CopyTo);

                Length = TotalCount + TailLength;
            }
        }

        public void Reverse()
        {
            AsSpan().Sort();
        }

        public void Sort()
        {
            AsSpan().Sort();
        }


        public T[] ToArray()
        {
            return AsSpan().ToArray();
        }


        public void EnsureCapacity(int Capacity)
        {
            Extend(Capacity);
        }

        public void Resize(int NewSize)
        {
            Length = Math.Max(0, NewSize);
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
            return GetEnumerator(0, Length);
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
                    TotalIndex, LocalIndex, Buffer,
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