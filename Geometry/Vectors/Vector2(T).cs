using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Zion.Geometry
{
    public struct Vector2<T> : IEquatable<Vector2<T>>, IEqualityComparer<Vector2<T>>
        where T : unmanaged, INumber<T>
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

        #region Operators
        #region MathOperators
        public static Vector2<T> operator +(Vector2<T> A, Vector2<T> B)
        {
            return new Vector2<T>(A.X + B.X, A.Y + B.Y);
        }

        public static Vector2<T> operator -(Vector2<T> A, Vector2<T> B)
        {
            return new Vector2<T>(A.X - B.X, A.Y - B.Y);
        }

        public static Vector2<T> operator *(Vector2<T> A, T B)
        {
            return new Vector2<T>(A.X * B, A.Y * B);
        }

        public static Vector2<T> operator /(Vector2<T> A, T B)
        {
            return new Vector2<T>(A.X / B, A.Y / B);
        }

        public static Vector2<T> operator &(Vector2<T> A, Axes B)
        {
            return new Vector2<T>
            (
                B.HasFlag(Axes.X) ? A.X : T.Zero,
                B.HasFlag(Axes.Y) ? A.Y : T.Zero
            );
        }

        public static Vector2<T> operator ++(Vector2<T> Value)
        {
            Value.X++;
            Value.Y++;
            return Value;
        }

        public static Vector2<T> operator --(Vector2<T> Value)
        {
            Value.X--;
            Value.Y--;
            return Value;
        }

        public static Vector2<T> operator -(Vector2<T> Value)
        {
            return new Vector2<T>(-Value.X, -Value.Y);
        }
        #endregion

        #region ComparisonOperators
        public static bool operator ==(Vector2<T> A, Vector2<T> B)
        {
            return A.X == B.X && A.Y.Equals(B.Y);
        }

        public static bool operator !=(Vector2<T> A, Vector2<T> B)
        {
            return A.X != B.X || A.Y != B.Y;
        }

        public static bool operator <(Vector2<T> A, Vector2<T> B)
        {
            return A.X < B.X && A.Y < B.Y;
        }

        public static bool operator >(Vector2<T> A, Vector2<T> B)
        {
            return A.X > B.X && A.Y > B.Y;
        }

        public static bool operator <=(Vector2<T> A, Vector2<T> B)
        {
            return A.X <= B.X && A.Y <= B.Y;
        }

        public static bool operator >=(Vector2<T> A, Vector2<T> B)
        {
            return A.X >= B.X && A.Y >= B.Y;
        }
        #endregion
        #endregion

        #region OverrideMethods
        public readonly override string ToString()
        {
            return $"[{X}; {Y}]";
        }

        public readonly override bool Equals([NotNullWhen(true)] object? Object)
        {
            return Object is Vector2<T> Value && this == Value;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        #endregion

        #region IEquatable & IEqualityComparer
        public bool Equals(Vector2<T> Other)
        {
            return this == Other;
        }

        public bool Equals(Vector2<T> A, Vector2<T> B)
        {
            return A == B;
        }

        public int GetHashCode([DisallowNull] Vector2<T> Object)
        {
            return Object.GetHashCode();
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