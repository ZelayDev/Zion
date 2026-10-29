namespace Zion.ADF
{
    internal sealed class AutoWriteStrategy<T> : IWriteStrategy<T>
    {
        private readonly AutoWriter<T> Writer;

        public AutoWriteStrategy(AutoWriter<T> Writer)
        {
            this.Writer = Writer.NotNull();
        }


        public void WriteData(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            Writer(Context, Target, Value);
        }
    }
}