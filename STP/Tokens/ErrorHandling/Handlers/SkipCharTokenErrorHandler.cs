namespace Zion.STP
{
    public readonly struct SkipCharTokenErrorHandler : ITokenErrorHandler
    {
        public static readonly SkipCharTokenErrorHandler Instance = new();

        public void Handle(ref TextSource Source, out ErrorToken Token)
        {
            Source.MoveNext();
            Token = new ErrorToken { Length = 1 };
        }
    }
}