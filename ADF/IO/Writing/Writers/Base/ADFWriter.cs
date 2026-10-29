namespace Zion.ADF
{
    public sealed class ADFWriter : BaseADFWriter
    {
        #region Data
        public  readonly Stream BaseStream;
        private readonly BinaryWriter Writer;

        private int LastPosition = 0;

        #endregion

        #region Constructors
        public ADFWriter(Stream Stream, ADFWritingOptions? Options = null) : base(new ADFWritingContext(Options))
        {
            if (!Stream.NotNull().CanWrite)
            {
                throw new InvalidOperationException("Stream can not write");
            }
            BaseStream = Stream;
            Writer = new(Stream);
        }

        #endregion

        #region PublicMethods
        public void Flush()
        {
            WriteHeader();
            WritePage();
        }

        #endregion

        #region OverrideMethods
        protected override StreamGroup GetWriteTarget(string Name, in uint NameId, in uint FormatId)
        {
            return DataRegistry.Contains(NameId)
                ? throw new ADFRepeatedNameException(Name)
                : base.GetWriteTarget(Name, in NameId, in FormatId);
        }

        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            var Definition = new DataDefinition
            (
                FormatId,
                Context.CurrentPage,
                LastPosition
            );

            LastPosition = CurrentPosition;
            DataRegistry.Add(Name, NameId, Definition);

            if (TotalLength >= Options.MinPageSize)
            {
                Flush();
            }
        }

        protected override void OnDisposed()
        {
            Flush();
            Writer.Write(false);
        }

        #endregion

        #region PrivateMethods
        private void WriteHeader()
        {
            var Options = this.Options;

            if (!Options.WriteHeader || Context.CurrentPage != 0)
            {
                return;
            }

            var Header = new ADFHeader()
            {
                Compression = Options.Compression
                //Other parameters
            };

            Header.Write(Writer);
        }

        private void WritePage()
        {
            Writer.Write(true);

            WriteRegistries();

            Writer.Write((ushort)0);
            Writer.Write(TotalLength);

            Flush(BaseStream);

            Context.Arena.DisposeAll();
            Context.CurrentPage++;
            Context.CurrentPosition = 0;
        }

        private void WriteRegistries()
        {
            var Context = this.Context;

            foreach (var Info in Registries)
            {
                var Registry = Info.Registry;

                if (Registry.IsChanged)
                {
                    Writer.Write(Info.Id);

                    var Entry = WriteStrategies.GetEntry<IWritableRegistry>(Context, Registry.GetType());
                    var Group = new StreamGroup(Context.Arena);

                    Entry.Strategy.Write(Context, Group, Registry);
                }
            }
        }

        #endregion
    }
}