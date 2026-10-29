using System.Collections;

namespace Zion
{
    public sealed class EmptyEnumerator<T> : IEnumerator<T>
    {
        public static readonly EmptyEnumerator<T> Instance = new();

        
        object IEnumerator.Current => null!;
        
        T IEnumerator<T>.Current => default!;
        
        
        public bool MoveNext()
        {
            return false;
        }

        public void Reset() { }
        
        public void Dispose() { }
    }
}