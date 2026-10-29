using System.Collections;

namespace Zion.ADF
{
    public readonly struct StreamGroup : IDisposable, IEnumerable<ArenaStream>
    {
        #region Data
        private readonly Reference<long>  _Length;
        private readonly List<StreamGroup> Groups;

        public readonly ArenaStream BaseStream;

        #endregion

        #region Properties
        public bool IsDisposed => BaseStream.IsDisposed;

        public int Count => Groups.Count;

        public long ChildsLength => Length - BaseStream.Length;

        public long Length
        {
            get => _Length.Value;
            set => _Length.Value = value;
        }

        #endregion

        #region Constructors
        public StreamGroup(Arena<byte> Arena)
            : this(Arena.NotNull().GetStream(1)) { }

        public StreamGroup(ArenaStream Stream)
        {
            BaseStream = Stream.NotNull();
            Groups     = new(0);
            _Length    = new();
        }

        private StreamGroup(ArenaStream BaseStream, List<StreamGroup> Groups, Reference<long> Length)
        {
            this.BaseStream = BaseStream;
            this.Groups     = Groups;
            this._Length    = Length;
        }

        #endregion

        #region PublicMethods
        public StreamGroup With(ArenaStream BaseStream)
        {
            return new(BaseStream.NotNull(), Groups, _Length);
        }

        public void Add(ArenaStream Stream)
        {
            Add(new StreamGroup(Stream));
        }

        public void Add(StreamGroup Group)
        {
            Groups.Add(Group);
            Length += Group.Length;
        }

        #endregion

        #region IDisposable
        public void Dispose()
        {
            foreach (var Stream in this)
            {
                Stream.Dispose();
            }
        }

        #endregion

        #region IEnumerable
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public IEnumerator<ArenaStream> GetEnumerator()
        {
            if (BaseStream is null) { yield break; }

            yield return BaseStream;

            foreach (var Group in Groups)
            {
                foreach (var Stream in Group)
                {
                    yield return Stream;
                }
            }
        }

        #endregion
    }
}