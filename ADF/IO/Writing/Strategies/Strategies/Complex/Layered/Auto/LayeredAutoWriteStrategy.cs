namespace Zion.ADF
{
    internal sealed class LayeredAutoWriteStrategy<T> : IWriteStrategy<T>
    {
        private readonly AutoWriter<T>[] Layers;

        public LayeredAutoWriteStrategy(List<ILayerWriteInfo> Layers)
        {
            this.Layers = CheckAndConvert(Layers);
        }


        public void WriteData(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i].Invoke(Context, Target, Value);
            }
        }


        private static AutoWriter<T>[] CheckAndConvert(List<ILayerWriteInfo> Layers)
        {
            var UsedNames = new HashSet<string>(Layers.Count * 6);
            var Writers = new AutoWriter<T>[Layers.Count];

            var Layer = 0;
            var Writer = Layers.Count - 1;

            while (Layer < Layers.Count)
            {
                var Info = (AutoType<T>)Layers[Layer];
                var Fields = Info.Schema.Fields;

                Writers[Writer] = Info.Writer;

                for (int j = 0; j < Fields.Length; j++)
                {
                    var Name = Fields[j].Name;
                    if (!UsedNames.Add(Name))
                    {
                        throw new ADFRepeatedNameException(Name);
                    }
                }

                Layer++;
                Writer--;
            }

            return Writers;
        }
    }
}