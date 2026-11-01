namespace Zion.STP
{
    public sealed class FilteredTextSource : TextSource
    {
        private readonly TextSource       Source;
        private readonly Func<char, bool> Filter;

        public override char Current => Source.Current;
        public override bool IsEnd   => Source.IsEnd;


        public FilteredTextSource(TextSource Source, Func<char, bool> Filter)
        {
            this.Source = Source.NotNull();
            this.Filter = Filter.NotNull();

            Update();
        }


        public override TextSource BeginNew()
        {
            return new FilteredTextSource(Source.BeginNew(), Filter);
        }

        public override void MoveNext()
        {
            Source.MoveNext();
            Update();
        }


        public void Reset()
        {
            Update();
        }

        private void Update()
        {
            while (!IsEnd && !Filter(Current))
            {
                Source.MoveNext();
            }
        }
    }
}