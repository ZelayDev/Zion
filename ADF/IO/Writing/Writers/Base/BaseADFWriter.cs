namespace Zion.ADF
{
    public abstract partial class BaseADFWriter : IDisposable
    {
        #region Data
        private protected readonly ADFWritingContext Context;
        private protected readonly StreamGroup Data;

        #endregion

        #region Properties
        protected ADFWritingOptions Options => Context.Options;

        private protected WritableRegistries   Registries => Context.Registries;
        private protected WriteStrategies WriteStrategies => Context.WriteStrategies;

        private protected TypeAssociation TypeAssociation => Registries.TypeAssociation;
        private protected ReferenceIdsRegistry References => Registries.References;
        private protected DataRegistry       DataRegistry => Registries.DataRegistry;
        private protected StringIdRegistry StringRegistry => Registries.StringRegistry;
        private protected FormatRegistry   FormatRegistry => Registries.FormatRegistry;

        public bool IsDisposed { get; private set; }

        public long TotalLength => Data.Length;

        public int CurrentPosition => Data.BaseStream.Position;

        #endregion

        #region Constructors
        internal BaseADFWriter(BaseADFWriter Base) : this(Base.Context) { }

        internal BaseADFWriter(BaseADFWriter Base, StreamGroup Target) : this(Base.Context, Target) { }

        internal BaseADFWriter(ADFWritingContext Context) : this(Context, new(Context.Arena.GetStream(1))) { }

        internal BaseADFWriter(ADFWritingContext Context, StreamGroup Target)
        {
            this.Context = Context.NotNull();
            this.Data = Target;
        }

        #endregion

        #region AbstractMethods
        protected virtual StreamGroup GetWriteTarget(string Name, in uint NameId, in uint FormatId) => Data;

        protected virtual void OnWrited(string Name, in uint NameId, in uint FormatId) { }

        protected virtual void OnDisposed() { }

        #endregion

        #region PublicMethods
        public void Flush(Stream Destination)
        {
            foreach (var Stream in Data)
            {
                Stream.CopyTo(Destination);
            }
        }

        #endregion

        #region ProtectedMethods
        protected ArenaStream GetNewStream()
        {
            return Context.Arena.GetStream(1);
        }

        protected ArenaStream GetBaseStream()
        {
            return Data.BaseStream;
        }

        protected uint GetOrAddDeferred(Type Type)
        {
            return TypeAssociation.GetOrAddDeferred(Type, FormatRegistry);
        }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(BaseADFWriter));
            }
        }

        #endregion

        #region Writing
        public void Write<T>(string Name, T? Value)
        {
            ThrowIfDisposed();

            var NameId = StringRegistry.GetOrAdd(Name.NotNull());

            if (Value is null)
            {
                if (!Options.CanWriteNull)
                {
                    throw new ADFObjectIsNullException(Name);
                }

                var NullFormatId = GetOrAddDeferred(typeof(T));
                var NullTarget = GetWriteTarget(Name, in NameId, NullFormatId);
                
                NullTarget.BaseStream.WriteCompressedZero(Context);
                OnWrited(Name, in NameId, NullFormatId);
                return;
            }

            WriteStrategies.GetEntry<T>(Context, Value.GetType()).Deconstruct
            (
                out var FormatId,
                out var Strategy
            );

            var Target = GetWriteTarget(Name, in NameId, in FormatId);

            Strategy.Write(Context, Target, Value);
            OnWrited(Name, in NameId, in FormatId);
        }

        internal void WriteStrings(string Name, List<string> Strings)
        {
            var Span = Strings.AsSpan();

            var NameId = StringRegistry.GetOrAdd(Name);
            var Target = GetWriteTarget(Name, in NameId, ADFPrimitives.RawString | ADFPrimitives.Sequence);
            var Stream = SequenceWriteHelper.GetSubStream(Context, Target, Span.Length, Span.Length * 64);

            foreach (var String in Span)
            {
                Stream.Write(String);
            }

            Stream.TrimExcess();

            Target.Add(Stream);
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                Context.CurrentPosition += Data.Length;
                OnDisposed();
            }
        }

        #endregion

        #region PrivateMethods
        private static bool IsRootType(Type Type)
        {
            if (Type.IsValueType)
            {
                return true;
            }
            var BaseType = Type.BaseType;
            return BaseType is null || BaseType == typeof(object);
        }

        private static IEnumerable<Type> EnumerateHierarchy(Type Type)
        {
            if (!IsRootType(Type))
            {
                foreach (var Hierarchy in EnumerateHierarchy(Type.BaseType!))
                {
                    yield return Hierarchy;
                }
            }

            yield return Type;
        }

        #endregion
    }
}