namespace Zion
{
    public static class ArenaExtensions
    {
        extension(Arena<byte> Arena)
        {
            public ArenaStream GetStream(int Capacity)
            {
                return Arena.Allocate<ArenaStream>(Capacity);
            }
        }
    }
}