namespace Zion.ADF
{
    internal ref struct EnumeratorWriteContext<T> where T : unmanaged
    {
        public readonly string Name;
        public readonly uint FormatId;

        private readonly IEnumerator<T> Enumerator;
        private readonly StreamWriteAction<T> Writing;


        public EnumeratorWriteContext(string Name, uint FormatId, IEnumerator<T> Enumerator, StreamWriteAction<T> Writing)
        {
            this.Name = Name.NotNull();
            this.FormatId = FormatId | ADFPrimitives.Sequence;

            this.Enumerator = Enumerator;

            this.Writing = Writing.NotNull();
        }


        public void Write(ADFWritingContext Context, StreamGroup Target)
        {
            var Enumerator = this.Enumerator;
            var WriteSpan = Writing;

            var Link = Reference.CreateNewReference(Target.ChildsLength);
            var Stream = Context.Arena.GetStream(256);

            var Compression = Context.Compression;
            var Index = 0;
            var BufferIndex = 0;

            Span<T> Buffer = stackalloc T[32];

            Target.BaseStream.WriteCompressed(Context, Link);
            Stream.Position = 4;

            while (Enumerator.MoveNext())
            {
                Buffer[BufferIndex++] = Enumerator.Current;
                Index++;

                if (BufferIndex >= Buffer.Length)
                {
                    WriteSpan(Stream, Buffer, Compression);
                    BufferIndex = 0;
                }
            }

            if (BufferIndex > 0)
            {
                WriteSpan(Stream, Buffer.Slice(BufferIndex), Compression);
            }

            Stream.Position = 0;
            Stream.Write(Index);

            Target.Add(Stream);
        }
    }
}