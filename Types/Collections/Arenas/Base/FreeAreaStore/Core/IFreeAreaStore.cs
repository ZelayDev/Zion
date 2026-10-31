namespace Zion
{
    public interface IFreeAreaStore
    {
        public Segment Allocate(int Capacity);

        public Segment Expand(Segment Area, int Capacity);

        public Segment Trim(Segment Area, int Capacity);

        public void Release(Segment Area);

        public void Clear();
    }
}