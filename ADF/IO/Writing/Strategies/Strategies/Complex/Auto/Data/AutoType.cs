namespace Zion.ADF
{
    internal sealed class AutoType<T> : IAutoType<T>
    {
        public AutoWriter<T> Writer { get; }
        public TypeSchema Schema { get; }

        public AutoType(AutoWriter<T> Writer, TypeSchema Schema)
        {
            this.Writer = Writer;
            this.Schema = Schema;
        }
    }
}