﻿namespace StackExchange.Profiling.Data
{
    using System.Threading;

    /// <summary>
    /// This is a micro-cache; suitable when the number of terms is controllable (a few hundred, for example),
    /// and strictly append-only; you cannot change existing values. All key matches are on **REFERENCE**
    /// equality. The type is fully thread-safe.
    /// </summary>
    /// <typeparam name="TKey">the key type</typeparam>
    /// <typeparam name="TValue">the value type</typeparam>
    public class Link<TKey, TValue> where TKey : class
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Link{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="tail">
        /// The tail.
        /// </param>
        private Link(TKey key, TValue value, Link<TKey, TValue> tail)
        {
            this.Key = key;
            this.Value = value;
            this.Tail = tail;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public TValue Value { get; private set; }

        /// <summary>
        /// Gets the tail.
        /// </summary>
        public Link<TKey, TValue> Tail { get; private set; }

        /// <summary>
        /// try and return a value from the cache based on the key.
        /// the default value is returned if no match is found.
        /// An exception is not thrown.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>return true if a value is located.</returns>
        public static bool TryGet(Link<TKey, TValue> link, TKey key, out TValue value)
        {
            while (link != null)
            {
                if ((object)key == (object)link.Key)
                {
                    value = link.Value;
                    return true;
                }

                link = link.Tail;
            }

            value = default(TValue);
            return false;
        }

        /// <summary>
        /// try and return a value from the cache based on the key.
        /// the default value is returned if no match is found.
        /// An exception is not thrown.
        /// </summary>
        /// <param name="head">The head.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>return true if a value is located</returns>
        public static bool TryAdd(ref Link<TKey, TValue> head, TKey key, ref TValue value)
        {
            bool tryAgain;
            do
            {
                var snapshot = Interlocked.CompareExchange(ref head, null, null);
                TValue found;
                if (TryGet(snapshot, key, out found))
                {
                    // existing match; report the existing value instead
                    value = found;
                    return false;
                }

                var newNode = new Link<TKey, TValue>(key, value, snapshot);

                // did somebody move our cheese?
                tryAgain = Interlocked.CompareExchange(ref head, newNode, snapshot) != snapshot;
            }
            
            while (tryAgain);
            return true;
        }
    }
}
