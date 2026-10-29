namespace Zion.ADF
{
    internal readonly struct Reference
    {
        //{ [Offset ][1] } - Writed
        //{ [CacheUd][0] } - Cached (>= 2)
        //{ 0 } - Null

        public readonly ulong Id;
        public readonly DataDefinition Definition;

        public Reference(ulong Id, DataDefinition Definition)
        {
            this.Id = Id;
            this.Definition = Definition;
        }

        public static ulong CreateNewReference(long ObjectOffset)
        {
            return ((ulong)ObjectOffset << 1) | 1UL;
        }
    }
}