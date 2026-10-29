namespace Zion.ADF
{
    internal readonly struct ReadableRegistryInfo
    {
        public readonly ushort Id;
        public readonly IReadableRegistry Registry;

        public ReadableRegistryInfo(ushort Id, IReadableRegistry Registry)
        {
            ArgumentOutOfRangeException.ThrowIfZero(Id);
            this.Id = Id;
            this.Registry = Registry.NotNull();
        }
    }
}