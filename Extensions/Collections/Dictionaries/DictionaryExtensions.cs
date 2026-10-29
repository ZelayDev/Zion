namespace Zion
{
    public static class DictionaryExtensions
    {
        extension<TKey, TValue>(Dictionary<TKey, TValue> Dictionary) where TKey : notnull where TValue : notnull
        {
            /// <summary>
            /// Creates a new dictionary with keys and values swapped.
            /// </summary>
            /// <typeparam name="TKey">The type of keys in the original dictionary.</typeparam>
            /// <typeparam name="TValue">The type of values in the original dictionary.</typeparam>
            /// <param name="Dictionary">The source dictionary to reverse.</param>
            /// <returns>A new dictionary where values become keys and keys become values.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="Dictionary"/> is null.</exception>
            /// <exception cref="ArgumentException">Thrown if duplicate values exist in the original dictionary.</exception>
            public Dictionary<TValue, TKey> Reverse()
            {
                ArgumentNullException.ThrowIfNull(Dictionary);

                Dictionary<TValue, TKey> Result = new Dictionary<TValue, TKey>(Dictionary.Count);

                foreach (KeyValuePair<TKey, TValue> Item in Dictionary)
                {
                    Result.Add(Item.Value, Item.Key);
                }
                return Result;
            }
        }

        extension<TKey, TValue>(IDictionary<TKey, TValue> Dictionary)
        {
            public TValue GetValue(TKey Key, TValue Default)
            {
                return Dictionary.TryGetValue(Key, out TValue? Value) ? Value : Default;
            }

            public TValue GetValue(TKey Key, TValue Default, Func<TValue, TValue> Converter)
            {
                return Dictionary.TryGetValue(Key, out TValue? Value) ? Converter(Value) : Default;
            }


            public TValue AddAndReturn(TKey Key, TValue Value)
            {
                Dictionary.Add(Key, Value);
                return Value;
            }


            public TValue GetOrAdd(TKey Key, TValue NewValue)
            {
                if (Dictionary.TryGetValue(Key, out var Existing))
                {
                    return Existing;
                }
                Dictionary.Add(Key, NewValue);
                return NewValue;
            }

            public TValue GetOrAdd(TKey Key, Func<TValue> Fabric)
            {
                if (Dictionary.TryGetValue(Key, out var Existing))
                {
                    return Existing;
                }
                var NewValue = Fabric();
                Dictionary.Add(Key, NewValue);
                return NewValue;
            }

            public TValue GetOrAdd(TKey Key, Func<TKey, TValue> Fabric)
            {
                if (Dictionary.TryGetValue(Key, out var Existing))
                {
                    return Existing;
                }
                var NewValue = Fabric(Key);
                Dictionary.Add(Key, NewValue);
                return NewValue;
            }
        }
    }
}