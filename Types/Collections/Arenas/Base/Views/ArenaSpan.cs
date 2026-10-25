using System.Runtime.CompilerServices;

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
            if (Start < 0 || Start >= Region.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Start));
            }
            if (Start == 0)
            {
                return this;
            }

            return new ArenaSpan<T>(new ArenaRegion<T>(Region.Chunks, Start, Region.Length - Start));
        }

        public ArenaSpan<T> Slice(int Start, int Length)
        {
            if (Start < 0 || Start >= Region.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Start));
            }
            if (Length < 0 || Start + Length > this.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(Length));
            }

            return new ArenaSpan<T>(new ArenaRegion<T>(Region.Chunks, Start, Length - Start));
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
            throw new NotImplementedException(); //TODO
        }


        public ArenaItemEnumerator<T> GetEnumerator()
        {
            return new ArenaItemEnumerator<T>(Region);
        }
        #endregion
    }
}