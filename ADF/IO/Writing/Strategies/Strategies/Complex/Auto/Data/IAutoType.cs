namespace Zion.ADF
{
    internal interface IAutoType<in T> : ILayerWriteInfo
    {
        public AutoWriter<T> Writer { get; }
        public TypeSchema Schema { get; }
    }
}