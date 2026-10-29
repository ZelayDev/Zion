namespace Zion.ADF
{
    internal interface IWriteStrategy { }

    internal interface IWriteStrategy<in T> : IWriteStrategy
    {
        public void Write(ADFWritingContext Context, StreamGroup Target, T Value)
        {
            if (WriteStrategyHelper<T>.Setup(Context, Value, ref Target))
            {
                return;
            }
            WriteData(Context, Target, Value);
        }

        public void WriteData(ADFWritingContext Context, StreamGroup Target, T Value);
    }
}