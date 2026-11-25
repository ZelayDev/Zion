using System.Runtime.CompilerServices;

namespace Zion.Geometry
{
    public struct Vector2<T>
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

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
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