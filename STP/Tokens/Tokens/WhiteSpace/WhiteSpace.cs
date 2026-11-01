namespace Zion.STP
{
    public class WhiteSpaceToken : Token
    {
        public override int LineBreakCount { get; }

        public WhiteSpaceToken() { }

        public WhiteSpaceToken(int LineBreakCount)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(LineBreakCount);
            this.LineBreakCount = LineBreakCount;
        }
    }

    public readonly struct WhiteSpaceTokenReader : ITokenReader
    {
        public bool Read(ref TextSource Source, out Token Token)
        {
            var Length = 0;
            var LineBreakCount = 0;

            while (!Source.IsEnd && Char.IsWhiteSpace(Source.Current))
            {
                if (Source.Current == '\n')
                {
                    LineBreakCount++;
                }

                Length++;
                Source.MoveNext();
            }

            return WhiteSpaceToken.TryCreate(Length, () => new WhiteSpaceToken(LineBreakCount), out Token);
        }
    }
}