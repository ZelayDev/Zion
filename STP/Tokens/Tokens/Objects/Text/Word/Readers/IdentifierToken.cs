namespace Zion.STP
{
    public readonly struct IdentifierTokenReader : ITokenReader
    {
        public bool AllowDigits { get; init; } = true;

        public IdentifierTokenReader() { }

        public bool Read(ref TextSource Source, out Token Token)
        {
            Token = default!;
            var Chars = new List<char>(20);

            if (Source.CurrentIs(Char => Char == '_' || Char.IsEnglish()))
            {
                Chars.Add(Source.Current);
                Source.MoveNext();
            }
            else
            {
                return false;
            }

            while (!Source.IsEnd)
            {
                var Current = Source.Current;

                if (Current == '_' || Current.IsEnglish() || (AllowDigits && Current.IsDigit()))
                {
                    Chars.Add(Current);
                    Source.MoveNext();
                    continue;
                }

                return false;
            }

            if (Chars.Count == 0)
            {
                return false;
            }

            Token = new WordToken
            {
                Length = Chars.Count,
                Value = Chars.Create()
            };
            return true;
        }
    }
}