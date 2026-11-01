namespace Zion.STP
{
    public sealed class OffsetToken : WhiteSpaceToken
    {
        public readonly int Offset;
    }

    public readonly struct OffsetTokenReader : ITokenReader
    {
        private readonly int SingleIndent;

        public OffsetTokenReader(int SingleIndent = 3)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(SingleIndent);
            this.SingleIndent = SingleIndent;
        }


        public bool Read(ref TextSource Source, out Token Token)
        {
            var Length = 0;
            var SingleIndent = this.SingleIndent;

            bool BeginsIndent(TextSource Source)
            {
                for (var i = 0; i < SingleIndent && !Source.IsEnd; i++)
                {
                    if (Source.Current != ' ')
                    {
                        return false;
                    }
                    Source.MoveNext();
                }

                return true;
            }

            while (BeginsIndent(Source))
            {
                Length += SingleIndent;
            }

            return OffsetToken.TryCreate(Length, out Token);
        }
    }
}