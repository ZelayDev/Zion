namespace Zion.ADF
{
    internal sealed class LayeredMixedWriteStrategy<T> : IWriteStrategy<T>
    {
        private readonly ILayerWriteInfo[] Layers;


        public LayeredMixedWriteStrategy(List<ILayerWriteInfo> Layers)
        {
            this.Layers = Layers.ToArray();
        }


        public void WriteData(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            using var Writer = new ADFMixedLayersWriter<T>
            (
                Context,
                Target,
                Layers,
                Context.Registries.TypeAssociation[Value!.GetType()]
            );
            Writer.Serialize(Value);
        }
    }
}