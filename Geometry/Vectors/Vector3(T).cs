using System.Runtime.CompilerServices;

namespace Zion.Geometry
{
    public struct Vector3<T>
    {
        #region Data
        public T X, Y, Z;
        #endregion

        #region Constructors
        public Vector3(T Axes)
        {
            X = Axes;
            Y = Axes;
            Z = Axes;
        }

        public Vector3(T X, T Y, T Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        #endregion

        #region OverrideMethods
        public override string ToString()
        {
            return $"[{X}; {Y}; {Z}]";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
        #endregion

        #region PublicMethods
        public static int GetByteCount()
        {
            return Unsafe.SizeOf<Vector3<T>>();
        }
        #endregion
    }
}