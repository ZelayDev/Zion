namespace Zion.STP
{
    public sealed class TabOffsetToken : WhiteSpaceToken { }

    public readonly struct TabOffsetTokenReader : ITokenReader
    {
        public bool Read(ref TextSource Source, out Token Token)
        {
            var TabCount = Source.CountOf('\t', out Source);

            if (TabCount != 0)
            {
                Token = new TabOffsetToken { Length = TabCount };
                return true;
            }

            Token = default!;
            return false;
        }
    }
}