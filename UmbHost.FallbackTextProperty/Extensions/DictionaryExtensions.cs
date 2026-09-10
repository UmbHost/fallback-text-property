using System;
using System.Collections.Generic;

namespace UmbHost.FallbackTextProperty.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionaryA, Dictionary<TKey, TValue> dictionaryB)
        {
            foreach (var pair in dictionaryB)
            {
                dictionaryA[pair.Key] = pair.Value;
            }
        }
    }
}
