using System.Collections;

namespace Zion
{
    public sealed class ListView<T> : IEnumerable<T>
    {
        private readonly IList<T> Source;

        public int Count => Source.Count;

        public ListView(IList<T> Source)
        {
            this.Source = Source.NotNull();
        }

        public T this[int   Index] => Source[Index];
        public T this[Index Index] => Source[Index];


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Source.GetEnumerator();
        }
    }
}