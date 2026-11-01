using System.Runtime.CompilerServices;

namespace Zion
{
    public sealed class BitmapReleasedStore : IFreeAreaStore
    {
        private const int BinaryGroupSize = 3;
        private const int GroupSize       = 1 << 3;


        private BitArray Bitmap = BitArray.Empty;

        private int UsedGroups;


        public Segment Allocate(int Capacity)
        {
            var GroupCount = GetGroupCount(Capacity);

            if (Bitmap.TryFindShortestSequence(GroupCount, false, out var Sequence))
            {
                Bitmap.Fill(Sequence.Start, GroupCount, true);
                UsedGroups += GroupCount;
                return new Segment(Sequence.Start << BinaryGroupSize, Capacity);
            }

            var FirstGroup = UsedGroups;
            var LastGroup = FirstGroup + GroupCount;

            EnsureCapacity(LastGroup);
            Bitmap.Fill(FirstGroup, GroupCount, true);
            UsedGroups = LastGroup;

            return new Segment(FirstGroup << BinaryGroupSize, Capacity);
        }

        public Segment Expand(Segment Area, int Capacity)
        {
            var Start = Area.Start >> BinaryGroupSize;
            var OldLength = GetGroupCount(Area.End) - Start;
            var NewLength = GetGroupCount(Area.Start + Capacity) - Start;
            var Extra = NewLength - OldLength;

            var TailStart = GetGroupCount(Area.End);

            if (Bitmap.Contains(TailStart, Extra, false))
            {
                Bitmap.Fill(TailStart, Extra, true);
                UsedGroups += Extra;
                return new Segment(Area.Start, Capacity);
            }

            Bitmap.Fill(Start, OldLength, false);
            UsedGroups -= OldLength;
            return Allocate(Capacity);
        }

        public Segment Trim(Segment Area, int Capacity)
        {
            var Start = Area.Start << BinaryGroupSize;
            var OldLength = GetGroupCount(Area.End) - Start;
            var NewLength = GetItemCount(Area.Start + Capacity) - Start;

            if (Start + OldLength == UsedGroups)
            {
                UsedGroups = Start + NewLength;
                EnsureCapacity(UsedGroups);
                return new Segment(Area.Start, Capacity);
            }

            Bitmap.Fill(Start + OldLength, NewLength - OldLength, false);

            return new Segment(Area.Start, Capacity);
        }

        public void Release(Segment Area)
        {
            var Start = Area.Start >> BinaryGroupSize;
            var Length = GetGroupCount(Area.End) - Start;

            if (Start + Length == UsedGroups)
            {
                UsedGroups = Start;
            }

            Bitmap.Fill(Start, Length, false);
        }

        public void Clear()
        {
            UsedGroups = 0;
            Bitmap.Clear();
        }


        private void EnsureCapacity(int GroupCount)
        {
            const int BinaryGroupInChunk = Arena<byte>.BinaryChunkSize - BinaryGroupSize;
            const int GroupInChunk = 1 << BinaryGroupInChunk;

            var RoundedGroups = (GroupCount + GroupInChunk - 1) & ~(GroupInChunk - 1);

            if (Bitmap.Length < RoundedGroups)
            {
                BitArray.Resize(ref Bitmap!, RoundedGroups);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetGroupCount(int Count)
        {
            return (Count + GroupSize - 1) >> BinaryGroupSize;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetItemCount(int GroupCount)
        {
            return GroupCount << BinaryGroupSize;
        }
    }
}