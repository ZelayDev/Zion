using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zion
{
    public readonly ref struct ArenaSpan<T>
    {
        #region Data
        private readonly ArenaRegion<T> Region;

        public int Length => Region.Length;

        public ArenaChunkEnumerator<T> Chunks => new(Region);
        #endregion

        #region Constructors
        internal ArenaSpan(ArenaRegion<T> Region)
        {
            this.Region = Region;
        }
        #endregion
        
        #region Indexers
        public T this[int Index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Region[Index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Region[Index] = value;
        }

        public T this[Index Index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Region[Index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)] set => Region[Index] = value;
        }
        #endregion

        #region PublicMethods
        public ArenaSpan<T> Slice(int Start)
        {
            return new ArenaSpan<T>(Region.Slice(Start));
        }

        public ArenaSpan<T> Slice(int Start, int Count)
        {
            return new ArenaSpan<T>(Region.Slice(Start, Count));
        }


        public void Sort(IComparer<T>? Comparer = null)
        {
            throw new NotImplementedException(); //TODO
        }

        public void Reverse()
        {
            throw new NotImplementedException(); //TODO
        }


        public int IndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            
        }

        public int LastIndexOf(T Item, IEqualityComparer<T>? Comparer = null)
        {
            
        }
        
        
        public int FindIndex(Predicate<T> Match)
        {
            
        }

        public int FindLastIndex(Predicate<T> Match)
        {
            
        }


        public int BinarySearch(T Item, IComparer<T>? Comparer = null)
        {
            
        }
        
        public T? Find(Predicate<T> Match)
        {
            var Index = FindIndex(Match);
            return Index == -1
                ? default
                : Region[Index];
        }

        public T? FindLast(Predicate<T> Match)
        {
            var Index = FindLastIndex(Match);
            return Index == -1
                ? default
                : Region[Index];
        }

        public List<T> FindAll(Predicate<T> Match)
        {
            
        }
        
        
        public bool Exists(Predicate<T> Match)
        {
            
        }
        
        public bool Contains(T Item, IEqualityComparer<T>? Comparer = null)
        {
            
        }
        
        
        public bool Remove(T Item, IEqualityComparer<T>? Comparer = null)
        {
            
        }


        public void ForEach(Action<T> Action)
        {
            
        }

        public bool TrueForAll(Predicate<T> Match)
        {
            
        }
        
        
        public void CopyTo(Span<T> Destination)
        {
            //TODO
        }

        public void CopyTo(ArenaSpan<T> Destination)
        {
            
        }
        
        
        public T[] ToArray()
        {
            var Array = new T[Length];
            CopyTo(Array);
            return Array;
        }

        public List<T> ToList()
        {
            var List = new List<T>(Length);
            var Span = CollectionsMarshal.AsSpan(List);
            CollectionsMarshal.SetCount(List, Length);
            CopyTo(Span);
            return List;
        }


        public ArenaItemEnumerator<T> GetEnumerator()
        {
            return new ArenaItemEnumerator<T>(Region);
        }
        #endregion
    }
}