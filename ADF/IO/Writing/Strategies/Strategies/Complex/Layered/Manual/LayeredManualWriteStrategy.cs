namespace Zion.ADF
{
    internal sealed class LayeredManualWriteStrategy<T> : IWriteStrategy<T>
    {
        private readonly IADFSerializer<T>[] Serializers;

        public LayeredManualWriteStrategy(List<ILayerWriteInfo> Layers)
        {
            Serializers = Cast(Layers);
        }


        public void WriteData(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            using var Writer = new ADFManualLayersWriter<T>
            (
                Context,
                Target,
                Serializers,
                Context.Registries.TypeAssociation[Value!.GetType()]
            );
            Writer.Serialize(Value);
        }


        private static IADFSerializer<T>[] Cast(List<ILayerWriteInfo> Layers)
        {
            var Serializers = new IADFSerializer<T>[Layers.Count];

            var Layer = 0;
            var Serializer = Layers.Count - 1;

            while (Layer < Layers.Count)
            {
                Serializers[Serializer] = (IADFSerializer<T>)Layers[Layer];

                Layer++;
                Serializer--;
            }

            return Serializers;
        }
    }
}