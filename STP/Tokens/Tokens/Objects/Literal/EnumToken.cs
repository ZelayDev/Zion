namespace Zion.STP
{
    public sealed class EnumToken<T> : ValueToken<T> where T : Enum { }

    public readonly struct EnumTokenReader<T> : ITokenReader where T : struct, Enum
    {
        private readonly Dictionary<string, T> Values;
        private readonly IdentifierTokenReader Reader;

        public EnumTokenReader()
        {
            Values = Enum.GetValues<T>().ToDictionary(Value => Value.ToString());
            Reader = new IdentifierTokenReader();
        }

        public bool Read(ref TextSource Source, out Token Token)
        {
            if (Reader.Read(ref Source, out var WordToken))
            {
                var Identifier = ((WordToken)WordToken).Value;

                if (Values.TryGetValue(Identifier, out var Value))
                {
                    Token = new EnumToken<T>
                    {
                        Length = Identifier.Length,
                        Value = Value
                    };
                    return true;
                }
            }

            Token = default!;
            return false;
        }
    }
}