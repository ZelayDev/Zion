using Zion.STP.Dynamic;

namespace Zion.STP
{
    public sealed class StringTextSource : TextDocument<IntPointer>
    {
        private readonly string Source;

        private int Index
        {
            get;
            set
            {
                if (value < Source.Length)
                {
                    _Current = Source[value];
                    _IsEnd = false;
                }
                else
                {
                    _IsEnd = true;
                }

                field = value;
            }
        }

        private char _Current;
        private bool _IsEnd;

        public override char Current => _Current;
        public override bool IsEnd   => _IsEnd;

        public override IntPointer CurrentPosition => new(Index);


        public StringTextSource(string Source) : this(Source, 0) { }

        public StringTextSource(string Source, int Start)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(Start);

            this.Source = Source.NotNull();
            Index = Start;
        }


        public override TextSource BeginFrom(IntPointer Position)
        {
            return new StringTextSource(Source, Position.Position);
        }

        public override StringTextSource BeginNewDocument()
        {
            return new StringTextSource(Source, Index);
        }

        public override void MoveNext()
        {
            Index++;
        }
    }
}