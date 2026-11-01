namespace Zion
{
    public sealed class BumpOnlyStore : IFreeAreaStore
    {
        private int Used;


        public Segment Allocate(int Capacity)
        {
            var Area = new Segment(Used, Capacity);
            Used += Capacity;
            return Area;
        }

        public Segment Expand(Segment Area, int Capacity)
        {
            if (Area.End == Used)
            {
                Used = Area.Start + Capacity;
                return new Segment(Area.Start, Capacity);
            }
            return Allocate(Capacity);
        }

        public Segment Trim(Segment Area, int Capacity)
        {
            if (Area.End == Used)
            {
                Used = Area.Start + Capacity;
            }
            return new Segment(Area.Start, Capacity);
        }

        public void Release(Segment Area)
        {
            if (Area.End == Used)
            {
                Used = Area.Start;
            }
        }

        public void Clear()
        {
            Used = 0;
        }
    }
}