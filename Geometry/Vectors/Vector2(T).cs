using System.Runtime.CompilerServices;

namespace Zion.Geometry
{
    public struct Vector2<T> : IEquatable<Vector2<T>>
    {
        #region Data
        public T X, Y;
        #endregion

        #region Constructors
        public Vector2(T Axes)
        {
            X = Axes;
            Y = Axes;
        }

        public Vector2(T X, T Y)
        {
            this.X = X;
            this.Y = Y;
        }
        #endregion

        #region OverrideMethods
        public readonly override string ToString()
        {
            return $"[{X}; {Y}]";
        }

        public readonly override bool Equals(object? Object)
        {
            return Object is Vector2<T> Other && Equals(Other);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        
        public readonly bool Equals(Vector2<T> Other)
        {
            var Comparer = EqualityComparer<T>.Default;
            return Comparer.Equals(X, Other.X) && Comparer.Equals(Y, Other.Y);
        }
        #endregion

        #region PublicMethods
        public static int GetByteCount()
        {
            return Unsafe.SizeOf<Vector2<T>>();
        }
        #endregion
    }
}