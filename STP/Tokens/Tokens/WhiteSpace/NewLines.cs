namespace Zion.STP
{
    public sealed class NewLinesToken : WhiteSpaceToken
    {
        public NewLinesToken() { }

        public NewLinesToken(int LineBreakCount) : base(LineBreakCount) { }

        public readonly struct NewLinesTokenReader : ITokenReader
        {
            public bool Read(ref TextSource Source, out Token Token)
            {
                var Length = 0;
                var LineCount = 0;

                while (true)
                {
                    if (Source.Begins("\n", out Source))
                    {
                        Length++;
                        LineCount++;
                    }
                    else if (Source.Begins("\r\n", out Source))
                    {
                        Length += 2;
                        LineCount++;
                    }
                    else
                    {
                        return NewLinesToken.TryCreate(Length, out Token);
                    }
                }
            }
        }
    }
}