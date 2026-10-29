namespace Zion
{
    public sealed class Reference<T>
    {
        public T Value;


        public Reference()
        {
            Value = default!;
        }

        public Reference(T Value)
        {
            this.Value = Value;
        }
    }
}