using System;

using System.Collections;

using System.Collections.Generic;

using System.Threading;
using System.Linq;

#if !CSHARP30
using System.Collections.Concurrent;
#endif

namespace MvcMiniProfiler
{
	internal interface IConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>
	{
		bool TryRemove(TKey key, out TValue value);
	}

#if CSHARP30
	internal static class EnumHelper
	{
		public static bool TryParse<T>(string value, out T e)
		{
			e = default(T);

			try
			{
				e = (T)Enum.Parse(typeof(T), value);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

	internal static class GuidHelper
	{
		public static bool TryParse(string value, out Guid id)
		{
			id = Guid.Empty;

			try
			{
				id = new Guid(value);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}

	internal static class Tuple
	{
		public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new Tuple<T1, T2>(item1, item2);
		}
	}

	internal class Tuple<T1, T2>
	{
		public Tuple(T1 item1, T2 item2)
		{
			Item1 = item1;
			Item2 = item2;
		}

		public T1 Item1 { get; private set; }
		public T2 Item2 { get; private set; }

		public override int GetHashCode()
		{
			return Item1.GetHashCode() ^ Item2.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			var other = obj as Tuple<T1, T2>;
			if (other == null)
			{
				return false;
			}

			var comparer1 = EqualityComparer<T1>.Default;
			var comparer2 = EqualityComparer<T2>.Default;
			return comparer1.Equals(Item1, other.Item1)&& comparer2.Equals(Item2, other.Item2);
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// This is a dictionary that only implements the methods required by the <see cref="SqlProfiler"/>.  Concurrency is very simply implemented via a <see cref="ReaderWriterLockSlim"/>.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TValue">The type of the value.</typeparam>
	internal class MiniConcurrentDictionary<TKey, TValue> : IConcurrentDictionary<TKey, TValue>
	{
		private Dictionary<TKey, TValue> _dict = new Dictionary<TKey, TValue>();
		private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

		public TValue this[TKey key]
		{
			get 
			{
				_lock.EnterReadLock();
				try
				{
					return _dict[key];
				}
				finally
				{
					_lock.ExitReadLock();
				}
			}
			set 
			{ 
				_lock.EnterWriteLock();
				try
				{
					_dict[key] = value;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}
		
		public ICollection<TValue> Values
		{
			get
			{
				_lock.EnterReadLock();
				try
				{
					return _dict.Values.ToList();
				}
				finally
				{
					_lock.ExitReadLock();
				}
			}
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			var result = false;
			_lock.EnterReadLock();
			try
			{
				result = _dict.TryGetValue(key, out value);
			}
			finally
			{
				_lock.ExitReadLock();
			}

			return result;
		}

		public bool TryRemove(TKey key, out TValue value)
		{
			var result = false;
			_lock.EnterWriteLock();
			try
			{
				if (_dict.TryGetValue(key, out value))
				{
					result = _dict.Remove(key);
				}
			}
			finally
			{
				_lock.ExitWriteLock();
			}

			return result;
		}
		
		#region Not Implemented
		
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			throw new NotImplementedException();
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { throw new NotImplementedException(); }
		}
		public bool IsReadOnly
		{
			get { throw new NotImplementedException(); }
		}
		public bool ContainsKey(TKey key)
		{
			throw new NotImplementedException();
		}

		public void Add(TKey key, TValue value)
		{
			throw new NotImplementedException();
		}

		public bool Remove(TKey key)
		{
			throw new NotImplementedException();
		}

		public ICollection<TKey> Keys
		{
			get { throw new NotImplementedException(); }
		} 

		#endregion
	}
#else
	internal class MiniConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>, IConcurrentDictionary<TKey, TValue>
	{
		bool IConcurrentDictionary<TKey, TValue>.TryRemove(TKey key, out TValue value)
		{
			return base.TryRemove(key, out value);
		}
	}
#endif
}