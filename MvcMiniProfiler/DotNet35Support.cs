using System;
using System.Collections.Generic;

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
			throw new NotImplementedException();
		}

		public override bool Equals(object obj)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}
	}

	internal class MiniConcurrentDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IConcurrentDictionary<TKey, TValue>
	{
		public bool TryRemove(TKey key, out TValue value)
		{
			throw new NotImplementedException();
		}
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