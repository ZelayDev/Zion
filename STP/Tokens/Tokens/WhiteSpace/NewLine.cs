namespace Zion.STP
{
    public sealed class NewLineToken : WhiteSpaceToken
    {
        public NewLineToken() : base(1) { }
    }

    public readonly struct NewLineTokenReader : ITokenReader
    {
        public bool Read(ref TextSource Source, out Token Token)
        {
            if (Source.Begins("\n", out Source))
            {
                Token = new NewLineToken { Length = 1 };
                return true;
            }
            if (Source.Begins("\r\n", out Source))
            {
                Token = new NewLineToken { Length = 2 };
                return true;
            }

            Token = null!;
            return false;
        }
    }
}