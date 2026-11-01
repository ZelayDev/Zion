namespace Zion.ADF
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ADFNameAttribute : ADFAttribute
    {
        public readonly string Name;

        public ADFNameAttribute(string Name)
        {
            this.Name = Name.NotNull();
        }
    }
}