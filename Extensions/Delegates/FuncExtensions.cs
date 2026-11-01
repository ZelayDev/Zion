namespace Zion
{
    public static class FuncExtensions
    {
        public static bool Try<T>(this Func<T> Function, out T Result)
        {
            try
            {
                Result = Function();
                return true;
            }
            catch
            {
                Result = default!;
                return false;
            }
        }

        public static bool Try<TIn, TOut>(this Func<TIn, TOut> Function, TIn In, out TOut Result)
        {
            try
            {
                Result = Function(In);
                return true;
            }
            catch
            {
                Result = default!;
                return false;
            }
        }


        public static Func<bool> Not(this Func<bool> Function)
        {
            return () => !Function();
        }

        public static Func<T1, bool> Not<T1>(this Func<T1, bool> Function)
        {
            return T1 => !Function(T1);
        }

        public static Func<T1, T2, bool> Not<T1, T2>(this Func<T1, T2, bool> Function)
        {
            return (T1, T2) => !Function(T1, T2);
        }

        public static Func<T1, T2, T3, bool> Not<T1, T2, T3>(this Func<T1, T2, T3, bool> Function)
        {
            return (T1, T2, T3) => !Function(T1, T2, T3);
        }

        public static Func<T1, T2, T3, T4, bool> Not<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> Function)
        {
            return (T1, T2, T3, T4) => !Function(T1, T2, T3, T4);
        }

        public static Func<T1, T2, T3, T4, T5, bool> Not<T1, T2, T3, T4, T5>(
            this Func<T1, T2, T3, T4, T5, bool> Function)
        {
            return (T1, T2, T3, T4, T5) => !Function(T1, T2, T3, T4, T5);
        }

        public static Func<T1, T2, T3, T4, T5, T6, bool> Not<T1, T2, T3, T4, T5, T6>(
            this Func<T1, T2, T3, T4, T5, T6, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6) => !Function(T1, T2, T3, T4, T5, T6);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, bool> Not<T1, T2, T3, T4, T5, T6, T7>(
            this Func<T1, T2, T3, T4, T5, T6, T7, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7) => !Function(T1, T2, T3, T4, T5, T6, T7);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> Not<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7, T8) => !Function(T1, T2, T3, T4, T5, T6, T7, T8);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> Not<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7, T8, T9) => !Function(T1, T2, T3, T4, T5, T6, T7, T8, T9);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> Not<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) => !Function(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> Not<T1, T2, T3, T4, T5, T6, T7, T8, T9,
            T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) =>
                !Function(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> Not<T1, T2, T3, T4, T5, T6, T7, T8,
            T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) => !Function(T1, T2, T3, T4, T5, T6, T7, T8, T9,
                T10, T11, T12);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> Not<T1, T2, T3, T4, T5, T6, T7,
            T8, T9, T10, T11, T12, T13>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) => !Function(T1, T2, T3, T4, T5, T6, T7, T8,
                T9, T10, T11, T12, T13);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> Not<T1, T2, T3, T4, T5,
            T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) => !Function(T1, T2, T3, T4, T5, T6,
                T7, T8, T9, T10, T11, T12, T13, T14);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> Not<T1, T2, T3, T4,
            T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) => !Function(T1, T2, T3, T4, T5,
                T6, T7, T8, T9, T10, T11, T12, T13, T14, T15);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> Not<T1, T2, T3,
            T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> Function)
        {
            return (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) => !Function(T1, T2, T3, T4,
                T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16);
        }
    }
}