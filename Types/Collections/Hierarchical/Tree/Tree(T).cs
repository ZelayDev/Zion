using System.Collections;
using System.Text;
using System.Text.Json.Serialization;
using Zion.Serialization;

namespace Zion
{
    [Serializable]
    public class Tree<T> : IList<Tree<T>>, IBinarySerializable<Tree<T>, T>, IColorText
    {
        #region Data
        [JsonPropertyName("Value") ] public  T             Value  { get; set; }
        [JsonPropertyName("Parent")] public  Tree<T>?      Parent { get; private set; }
        [JsonPropertyName("Childs")] private List<Tree<T>> Childs
        {
            get;
            set
            {
                field?.ForEach(static Item => Item.Parent = null);
                value. ForEach(Item => Item.Parent = this);
                field = value.NotNull();
            }
        }

        #endregion

        #region Properties
        public Tree<T> Root    => Parent is null ? this : Parent.Root;

        public int Count       => Childs.Count;

        public bool IsRoot     => Parent is null;
        public bool IsEmpty    => Childs.Count == 0;
        public bool IsReadOnly => false;

        #endregion

        #region Constructors
        public Tree(T Value)
            : this(Value, new List<Tree<T>>(0), null) { }

        public Tree(T Value, Tree<T> Parent)
            : this(Value, new List<Tree<T>>(0), Parent) { }

        public Tree(T Value, IList<T> Childs, Tree<T>? Parent = null)
            : this(Value, List.ConvertAll(Childs, ToTree), Parent) { }

        public Tree(T Value, IList<Tree<T>> Childs, Tree<T>? Parent = null)
            : this(Value, Childs.ToList(), Parent) { }

        private Tree(T Value, List<Tree<T>> Childs, Tree<T>? Parent = null)
        {
            this.Value = Value;
            this.Childs = Childs.NotNull();
            this.Parent = Parent;
        }

        #endregion

        #region Indexers
        public Tree<T> this[int Index]
        {
            get => Childs[Index];
            set
            {
                Childs[Index].Parent = null;
                Childs[Index] = value;
                value.Parent = this;
            }
        }

        public Tree<T> this[Index Index]
        {
            get => Childs[Index];
            set => this[Index.GetOffset(Count)] = value;
        }

        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            StringBuilder Result = new StringBuilder();
            BuildTreeString(Result, string.Empty, true);
            return Result.ToString().TrimEnd();
        }

        public string ToColorString()
        {
            return ToColorString(new RGBColor(67, 211, 224));
        }

        public string ToColorString(RGBColor LineColor)
        {
            StringBuilder Result = new StringBuilder();
            BuildTreeColorString(Result, string.Empty, true, LineColor);
            return Result.ToString().TrimEnd();
        }

        private void BuildTreeString(StringBuilder Builder, string Prefix, bool IsLast)
        {
            Builder.Append(Prefix);

            if (!IsRoot)
            {
                Builder.Append(IsLast ? "└──" : "├──");
            }

            Builder.AppendLine(Value?.ToString());

            if (!IsRoot)
            {
                Prefix += IsLast ? "   " : "│  ";
            }

            for (int i = 0; i < Childs.Count; i++)
            {
                bool LastChild = i == Childs.Count - 1;
                Childs[i].BuildTreeString(Builder, Prefix, LastChild);
            }
        }

        private void BuildTreeColorString(StringBuilder Builder, string Prefix, bool IsLast, RGBColor LineColor)
        {
            Builder.Append(Prefix);

            if (!IsRoot)
            {
                Builder.Append(IsLast ? new ColorText("└──", LineColor).ToString() : new ColorText("├──", LineColor));
            }

            Builder.AppendLine(Value?.ToString());

            if (!IsRoot)
            {
                Prefix += IsLast ? "   " : new ColorText("│  ", LineColor);
            }

            for (int i = 0; i < Childs.Count; i++)
            {
                bool LastChild = i == Childs.Count - 1;
                Childs[i].BuildTreeColorString(Builder, Prefix, LastChild, LineColor);
            }
        }

        #endregion

        #region Childs
        public void SetChilds(IList<Tree<T>> Childs)
        {
            this.Childs = Childs.ToList();
        }

        public void SetChilds(IList<T> Childs)
        {
            this.Childs = List.ConvertAll(Childs, ToTree);
        }

        public void SetChilds<I>(IList<I> Childs, Func<I, Tree<T>> Converter)
        {
            this.Childs = List.ConvertAll(Childs, Converter);
        }


        public Tree<T> AddIf(bool Condition, T Item)
        {
            if (Condition)
            {
                Add(Item);
            }
            return this;
        }

        public Tree<T> AddIf(bool Condition, Tree<T> Item)
        {
            if (Condition)
            {
                Add(Item);
            }
            return this;
        }


        public Tree<T> AddIf(Predicate<T> Condition, T Item)
        {
            if (Condition(Item))
            {
                Add(Item);
            }
            return this;
        }

        public Tree<T> AddIf(Predicate<Tree<T>> Condition, Tree<T> Item)
        {
            if (Condition(Item))
            {
                Add(Item);
            }
            return this;
        }


        public Tree<T> AddWhere(Func<T, bool> Condition, params IEnumerable<T> Items)
        {
            foreach (T Item in Items.Where(Condition))
            {
                Add(Item);
            }
            return this;
        }

        public Tree<T> AddWhere(Func<Tree<T>, bool> Condition, params IEnumerable<Tree<T>> Items)
        {
            foreach (Tree<T> Item in Items.Where(Condition))
            {
                Add(Item);
            }
            return this;
        }

        #endregion

        #region Parent
        public Tree<T>? GetParent(int Level)
        {
            TryGetParent(Level, out Tree<T>? Parent);
            return Parent;
        }

        public bool TryGetParent(int Level, out Tree<T>? Parent)
        {
            if (Level < 0)
            {
                Parent = null;
                return false;
            }

            Parent = this;

            if (Level == 0)
            {
                return true;
            }

            for (int i = 0; i < Level; i++)
            {
                Parent = Parent.Parent;

                if (Parent is null)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsAncestor(Tree<T> Tree)
        {
            ArgumentNullException.ThrowIfNull(Tree);

            Tree<T>? Current = this;

            while (Current is not null)
            {
                if (ReferenceEquals(Current, Tree))
                {
                    return true;
                }
                Current = Current.Parent;
            }

            return false;
        }

        #endregion

        #region ForEach
        public void ForEach(Action<T> Action)
        {
            ForEach((Tree<T> Tree) => Action(Tree.Value));
        }

        public void ForEach(Action<Tree<T>> Action)
        {
            Childs.ForEach(Action);
        }


        public void InvokeForAll(Action<T> Action)
        {
            Action(Value);
            foreach (Tree<T> Child in Childs)
            {
                Child.InvokeForAll(Action);
            }
        }

        public void InvokeForAll(Action<T, int> Action)
        {
            InvokeForAll(Action, 0);
        }

        private void InvokeForAll(Action<T, int> Action, int Level)
        {
            Action(Value, Level);
            int NextLevel = Level + 1;
            Childs.ForEach(Child => InvokeForAll(Action, NextLevel));
        }

        #endregion

        #region IList
        public void Add(T Item)
        {
            Add(ToTree(Item));
        }

        public void Add(Tree<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            if (IsAncestor(Item))
            {
                throw IsParentException();
            }
            Item.Parent = this;
            Childs.Add(Item);
        }

        public void Insert(int Index, T Item)
        {
            Insert(Index, new Tree<T>(Item, this));
        }

        public void Insert(int Index, Tree<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            if (IsAncestor(Item))
            {
                throw IsParentException();
            }
            Item.Parent = this;
            Childs.Insert(Index, Item);
        }


        public int IndexOf(Tree<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            return Childs.IndexOf(Item);
        }

        public bool Contains(Tree<T> Item)
        {
            ArgumentNullException.ThrowIfNull(Item);
            return Childs.Contains(Item);
        }


        public void CopyTo(Tree<T>[] Array, int ArrayIndex)
        {
            ArgumentNullException.ThrowIfNull(Array);
            Childs.CopyTo(Array, ArrayIndex);
        }


        public bool Remove(Tree<T> Item)
        {
            if (Childs.Remove(Item))
            {
                Item.Parent = null;
                return true;
            }
            return false;
        }

        public void RemoveAt(int Index)
        {
            Childs[Index].Parent = null;
            Childs.RemoveAt(Index);
        }

        public void Clear()
        {
            Childs.ForEach(static Item => Item.Parent = null);
            Childs.Clear();
        }


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Tree<T>> GetEnumerator()
        {
            return Childs.GetEnumerator();
        }

        #endregion

        #region IBinarySerializable
        public void Write(BinaryWriter Writer, Action<T> Write)
        {
            Write(Value);
            Writer.Write(Count);
            foreach (Tree<T> Child in Childs)
            {
                Child.Write(Writer, Write);
            }
        }

        public static Tree<T> Read(BinaryReader Reader, Func<T> Read)
        {
            return Read<T>(Reader, Read, null);
        }

        public static Tree<I> Read<I>(BinaryReader Reader, Func<I> Read, Tree<I>? Parent)
        {
            int Count = 0;
            Tree<I> Result = new Tree<I>
            (
                Read(),
                new List<Tree<I>>(Reader.ReadInt32().Out(out Count)),
                Parent
            );

            for (int i = 0; i < Count; i++)
            {
                Result.Add(Read<I>(Reader, Read, Result));
            }

            return Result;
        }

        #endregion

        #region PrivateMethods
        private static InvalidOperationException IsParentException()
        {
            return new InvalidOperationException("Cannot add a parent structure to its child elements");
        }

        private static Tree<I> ToTree<I>(I Item)
        {
            return new Tree<I>(Item);
        }

        #endregion
    }
}