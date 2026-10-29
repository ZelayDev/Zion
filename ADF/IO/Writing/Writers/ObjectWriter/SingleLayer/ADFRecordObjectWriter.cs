namespace Zion.ADF
{
    public sealed class ADFRecordObjectWriter : ADFObjectWriter
    {
        #region Data
        private readonly List<Parameter> Parameters;

        #endregion

        #region Constructors
        internal ADFRecordObjectWriter(ADFWritingContext Context, StreamGroup Target)
            : base(Context, Target)
        {
            Parameters = new();
        }

        #endregion

        #region OverrideMethods
        protected override StreamGroup GetWriteTarget(string Name, in uint NameId, in uint FormatId)
        {
            ThrowIfContains(Name, in NameId);
            return base.GetWriteTarget(Name, in NameId, in FormatId);
        }

        protected override void OnWrited(string Name, in uint NameId, in uint FormatId)
        {
            Parameters.Add(new Parameter(NameId, FormatId));
        }

        #endregion

        #region PublicMethods
        internal Parameter[] GetParameters()
        {
            Dispose();
            return Parameters.ToArray();
        }

        internal void Reset()
        {
            Parameters.Clear();
        }

        #endregion

        #region PrivateMethods
        private void ThrowIfContains(string Name, in uint NameId)
        {
            foreach (var Parameter in Parameters)
            {
                if (Parameter.NameId == NameId)
                {
                    throw new ADFRepeatedNameException(Name);
                }
            }
        }

        #endregion
    }
}