using System.Collections;

namespace Zion
{
    public sealed class ArrayView<T> : IEnumerable<T>
    {
        private readonly T[] Source;

        public readonly int Length;

        public ArrayView(T[] Source)
        {
            this.Source = Source.NotNull();
            Length = Source.Length;
        }

        public T this[int   Index] => Source[Index];
        public T this[Index Index] => Source[Index];


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((ICollection<T>)Source).GetEnumerator();
        }
    }
}