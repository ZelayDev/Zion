using System.Runtime.CompilerServices;

namespace Zion.Geometry
{
    public struct Vector3<T> : IEquatable<Vector3<T>>
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
        public readonly override string ToString()
        {
            return $"[{X}; {Y}; {Z}]";
        }

        public readonly override bool Equals(object? Object)
        {
            return Object is Vector3<T> Other && Equals(Other);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public readonly bool Equals(Vector3<T> Other)
        {
            var Comparer = EqualityComparer<T>.Default;
            return Comparer.Equals(X, Other.X) && Comparer.Equals(Y, Other.Y) && Comparer.Equals(Z, Other.Z);
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