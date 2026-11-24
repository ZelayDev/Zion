using System.Numerics;

namespace Zion.Geometry
{
    public static class Vector2Extensions
    {
        #region Operators
        #region Comparison
        extension<T>(Vector2<T> Value) where T : IEquatable<T>
        {
            public static bool operator ==(Vector2<T> A, Vector2<T> B)
            {
                return A.X.Equals(B.X) && A.Y.Equals(B.Y);
            }

            public static bool operator !=(Vector2<T> A, Vector2<T> B)
            {
                return !(A == B);
            }
        }

        extension<T>(Vector2<T> Value) where T : IComparisonOperators<T, T, bool>
        {
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
        }
        #endregion

        #region Math
        extension<T>(Vector2<T> Value) where T : IAdditionOperators<T, T, T>
        {
            public static Vector2<T> operator +(Vector2<T> A, Vector2<T> B)
            {
                return new Vector2<T>(A.X + B.X, A.Y + B.Y);
            }
        }

        extension<T>(Vector2<T> Value) where T : ISubtractionOperators<T, T, T>
        {
            public static Vector2<T> operator -(Vector2<T> A, Vector2<T> B)
            {
                return new Vector2<T>(A.X - B.X, A.Y - B.Y);
            }
        }

        extension<T>(Vector2<T> Value) where T : IMultiplyOperators<T, T, T>
        {
            public static Vector2<T> operator *(Vector2<T> A, T B)
            {
                return new Vector2<T>(A.X * B, A.Y * B);
            }
        }

        extension<T>(Vector2<T> Value) where T : IDivisionOperators<T, T, T>
        {
            public static Vector2<T> operator /(Vector2<T> A, T B)
            {
                return new Vector2<T>(A.X / B, A.Y / B);
            }
        }

        extension<T>(Vector2<T> Value) where T : IModulusOperators<T, T, T>
        {
            public static Vector2<T> operator %(Vector2<T> A, T B)
            {
                return new Vector2<T>(A.X % B, A.Y % B);
            }
        }

        extension<T>(Vector2<T> Value) where T : IUnaryNegationOperators<T, T>
        {
            public static Vector2<T> operator -(Vector2<T> A)
            {
                return new Vector2<T>(-A.X, -A.Y);
            }
        }
        #endregion

        #region Binary
        extension<T>(Vector2<T> Value) where T : IBitwiseOperators<T, T, T>
        {
            public static Vector2<T> operator &(Vector2<T> A, T B)
            {
                return new Vector2<T>(A.X & B, A.Y & B);
            }

            public static Vector2<T> operator |(Vector2<T> A, T B)
            {
                return new Vector2<T>(A.X | B, A.Y | B);
            }

            public static Vector2<T> operator ^(Vector2<T> A, T B)
            {
                return new Vector2<T>(A.X ^ B, A.Y ^ B);
            }

            public static Vector2<T> operator ~(Vector2<T> A)
            {
                return new Vector2<T>(~A.X, ~A.Y);
            }
        }

        extension<T>(Vector2<T> Value) where T : IShiftOperators<T, T, T>
        {
            public static Vector2<T> operator <<(Vector2<T> A, T B)
            {
                return new Vector2<T>(A.X << B, A.Y << B);
            }

            public static Vector2<T> operator >> (Vector2<T> A, T B)
            {
                return new Vector2<T>(A.X >> B, A.Y >> B);
            }

            public static Vector2<T> operator >>> (Vector2<T> A, T B)
            {
                return new Vector2<T>(A.X >>> B, A.Y >>> B);
            }
        }

        extension<T>(Vector2<T> Value) where T : INumberBase<T>
        {
            public static Vector2<T> Zero   => new(T.Zero);
            public static Vector2<T> Left   => new(-T.One, T.Zero);
            public static Vector2<T> Right  => new(T.One, T.Zero);
            public static Vector2<T> Down   => new(T.Zero, -T.One);
            public static Vector2<T> Up     => new(T.Zero, T.One);
            public static Vector2<T> OneOne => new(T.One);
            
            
            public int Radix => T.Radix;

            
            public static Vector2<T> operator ++(Vector2<T> A)
            {
                return new Vector2<T>(A.X + T.One, A.Y + T.One);
            }

            public static Vector2<T> operator --(Vector2<T> A)
            {
                return new Vector2<T>(A.X - T.One, A.Y - T.One);
            }
            
            public static Vector2<T> operator &(Vector2<T> A, Axes B)
            {
                return new Vector2<T>
                (
                    B.HasFlag(Axes.X) ? A.X : T.Zero,
                    B.HasFlag(Axes.Y) ? A.Y : T.Zero
                );
            }

            
            public Vector2<T> Abs()
            {
                return new Vector2<T>(T.Abs(Value.X), T.Abs(Value.Y));
            }

            
            public bool IsZero()
            {
                return Value.X == T.Zero && Value.Y == T.Zero;
            }
            
            public bool IsPositive()
            {
                return T.IsPositive(Value.X) && T.IsPositive(Value.Y);
            }

            public bool IsNegative()
            {
                return T.IsNegative(Value.X) && T.IsNegative(Value.Y);
            }
            
            
            public bool IsNormal()
            {
                return T.IsNormal(Value.X) && T.IsNormal(Value.Y);
            }
            
            public bool IsSubnormal()
            {
                return T.IsSubnormal(Value.X) && T.IsSubnormal(Value.Y);
            }
            
            
            public bool IsFinite()
            {
                return T.IsFinite(Value.X) && T.IsFinite(Value.Y);
            }
            
            public bool IsInfinity()
            {
                return T.IsInfinity(Value.X) && T.IsInfinity(Value.Y);
            }
            
            
            public bool IsNaN()
            {
                return T.IsNaN(Value.X) && T.IsNaN(Value.Y);
            } 
            
            public bool IsCanonical()
            {
                return T.IsCanonical(Value.X) && T.IsCanonical(Value.Y);
            }
            
            public bool IsComplexNumber()
            {
                return T.IsComplexNumber(Value.X) && T.IsComplexNumber(Value.Y);
            } 
            
            public bool IsRealNumber()
            {
                return T.IsRealNumber(Value.X) && T.IsRealNumber(Value.Y);
            }
            
            public bool IsImaginaryNumber()
            {
                return T.IsImaginaryNumber(Value.X) && T.IsImaginaryNumber(Value.Y);
            }
            
            
            public bool IsEvenInteger()
            {
                return T.IsEvenInteger(Value.X) && T.IsEvenInteger(Value.Y);
            }
            
            public bool IsOddInteger()
            {
                return T.IsOddInteger(Value.X) && T.IsOddInteger(Value.Y);
            }
            
            public bool IsInteger()
            {
                return T.IsInteger(Value.X) && T.IsInteger(Value.Y);
            } 
            
            
            public bool IsPositiveInfinity()
            {
                return T.IsPositiveInfinity(Value.X) && T.IsPositiveInfinity(Value.Y);
            } 
            
            public bool IsNegativeInfinity()
            {
                return T.IsNegativeInfinity(Value.X) && T.IsNegativeInfinity(Value.Y);
            }
            
            
            public static Vector2<T> MaxMagnitude(Vector2<T> A, Vector2<T> B)
            {
                return new
                (
                    T.MaxMagnitude(A.X, B.X),
                    T.MaxMagnitude(A.Y, B.Y)
                );
            }

            public static Vector2<T> MaxMagnitudeNumber(Vector2<T> A, Vector2<T> B)
            {
                return new
                (
                    T.MaxMagnitudeNumber(A.X, B.X),
                    T.MaxMagnitudeNumber(A.Y, B.Y)
                );
            }

            public static Vector2<T> MinMagnitude(Vector2<T> A, Vector2<T> B)
            {
                return new
                (
                    T.MinMagnitude(A.X, B.X),
                    T.MinMagnitude(A.Y, B.Y)
                );
            }

            public static Vector2<T> MinMagnitudeNumber(Vector2<T> A, Vector2<T> B)
            {
                return new
                (
                    T.MinMagnitudeNumber(A.X, B.X),
                    T.MinMagnitudeNumber(A.Y, B.Y)
                );
            }
        }

        extension<T>(Vector2<T> Value) where T : IRootFunctions<T>
        {
            public T Magnitude => T.Sqrt(Value.LengthSquared);

            public T LengthSquared => Value.X * Value.X + Value.Y * Value.Y;

            public Vector2<T> Normal
            {
                get
                {
                    var Zero = Vector2<T>.Zero;
                    if (Value == Zero)
                    {
                        return Zero;
                    }
                    
                    T Magnitude = Value.Magnitude;
                    return new Vector2<T>(Value.X / Magnitude, Value.Y / Magnitude);
                }
            }
        }
        #endregion
        #endregion
    }
}