using Line = System.Collections.Generic.List<Zion.STP.Token>;

namespace Zion.STP.Dynamic
{
    //Do: RowColumnTokenContainer realization
    public sealed class RowColumnTokenContainer : ITokenContainer<RowColumnPointer>
    {
        private readonly List<Line> Lines;

        public int Count { get; private set; }

        public RowColumnTokenContainer(int LineCapacity = 64)
        {
            Lines = new(Math.Max(LineCapacity, 8));
        }

        public void Add(Token Token)
        {
            Line LastLine = Lines[^1];
        }

        public void Overwrite(RowColumnPointer Start, int RemovedTokens, IEnumerable<Token> Tokens)
        {

        }

        public RowColumnPointer GetTokenStart(RowColumnPointer Position)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Token> EnumeratorFrom(RowColumnPointer Start)
        {
            throw new NotImplementedException();
        }
    }
}