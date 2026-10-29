using System.Collections;
using Zion.Serialization;

namespace Zion
{
    public sealed class ResizableArray<T> : IEnumerable<T>, IBinarySerializable<ResizableArray<T>, T>
    {
        #region Data
        private T[] Data;

        #endregion

        #region Properties
        public int Length
        {
            get => Data.Length;
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                if (value != Data.Length)
                {
                    Array.Resize(ref Data, value);
                }
            }
        }

        #endregion

        #region Constructors
        public ResizableArray(int Length)
        {
            Data = new T[Length];
        }

        public ResizableArray(T[] Array)
        {
            Data = Array.NotNull();
        }

        #endregion

        #region Indexers
        public T this[int Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }

        public T this[Index Index]
        {
            get => Data[Index];
            set => Data[Index] = value;
        }

        public T[] this[Range Range]
        {
            get => Data[Range];
        }

        #endregion

        #region PublicMethods
        public ResizableArray<T> Range(int Start, int Count)
        {
            ArgumentOutOfRangeException.ThrowIfWithout(Start, Length);
            ArgumentOutOfRangeException.ThrowIfBeyond(Start + Count, Length);

            T[] New = new T[Count];
            Array.Copy(Data, Start, New, 0, Count);

            return new ResizableArray<T>(New);
        }

        public void ForEach(Action<T> Action)
        {
            ArgumentNullException.ThrowIfNull(Action);

            foreach (T Item in Data)
            {
                Action(Item);
            }
        }

        public void EnsureCapacity(int Capacity)
        {
            if (Capacity > Length)
            {
                Length = Capacity;
            }
        }


        public ResizableArray<T> Clone()
        {
            return FromArray(Data);
        }

        public ResizableArray<T> FromArray(T[] Source)
        {
            return new ResizableArray<T>(ZArray.Clone(Source));
        }


        public T[] ToArray()
        {
            return ZArray.Clone(Data);
        }

        public List<T> ToList()
        {
            return new List<T>(Data);
        }

        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer, Action<T> Write)
        {
            Writer.Write(Length);
            foreach (T Item in Data)
            {
                Write(Item);
            }
        }

        public static ResizableArray<T> Read(BinaryReader Reader, Func<T> Read)
        {
            int Count = Reader.ReadInt32();
            T[] Array = new T[Count];

            for (int i = 0; i < Count; i++)
            {
                Array[i] = Read();
            }

            return new ResizableArray<T>(Array);
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public IEnumerator<T> GetEnumerator()
        {
            return Data.Enumerate();
        }

        #endregion
    }
}