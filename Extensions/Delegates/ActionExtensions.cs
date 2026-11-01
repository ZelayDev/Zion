namespace Zion
{
    public static class ActionExtensions
    {
        public static AsyncAction ToAsync(Action Action)
        {
            return () => Task.Run(Action);
        }

        public static AsyncAction<T> ToAsync<T>(this Action<T> Action)
        {
            return T => Task.Run(() => Action(T));
        }

        public static AsyncAction<T1, T2> ToAsync<T1, T2>(this Action<T1, T2> Action)
        {
            return (T1, T2) => Task.Run(() => Action(T1, T2));
        }

        public static AsyncAction<T1, T2, T3> ToAsync<T1, T2, T3>(this Action<T1, T2, T3> Action)
        {
            return (T1, T2, T3) => Task.Run(() => Action(T1, T2, T3));
        }

        public static AsyncAction<T1, T2, T3, T4> ToAsync<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> Action)
        {
            return (T1, T2, T3, T4) => Task.Run(() => Action(T1, T2, T3, T4));
        }

        public static AsyncAction<T1, T2, T3, T4, T5> ToAsync<T1, T2, T3, T4, T5>(
            this Action<T1, T2, T3, T4, T5> Action)
        {
            return (T1, T2, T3, T4, T5) => Task.Run(() => Action(T1, T2, T3, T4, T5));
        }

        public static AsyncAction<T1, T2, T3, T4, T5, T6> ToAsync<T1, T2, T3, T4, T5, T6>(
            this Action<T1, T2, T3, T4, T5, T6> Action)
        {
            return (T1, T2, T3, T4, T5, T6) => Task.Run(() => Action(T1, T2, T3, T4, T5, T6));
        }
    }
}