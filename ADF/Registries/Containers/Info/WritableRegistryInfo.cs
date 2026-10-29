namespace Zion.ADF
{
    internal readonly struct WritableRegistryInfo
    {
        public readonly ushort Id;
        public readonly IWritableRegistry Registry;

        public WritableRegistryInfo(ushort Id, IWritableRegistry Registry)
        {
            ArgumentOutOfRangeException.ThrowIfZero(Id);
            this.Id = Id;
            this.Registry = Registry.NotNull();
        }
    }
}