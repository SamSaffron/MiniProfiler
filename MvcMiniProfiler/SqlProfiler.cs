using System;
using System.Collections.Generic;
using System.Data.Common;
using MvcMiniProfiler.Data;
using System.Linq;

#if CSHARP30
using Tuple = MvcMiniProfiler.Tuple;
#else
using System.Collections.Concurrent;
#endif

namespace MvcMiniProfiler
{
	// TODO: refactor this out into MiniProfiler
    /// <summary>
    /// Contains helper code to time sql statements.
    /// </summary>
    public class SqlProfiler
    {
#if CSHARP30
		IConcurrentDictionary<Tuple<object, ExecuteType>, SqlTiming> _inProgress = new MiniConcurrentDictionary<Tuple<object, ExecuteType>, SqlTiming>();
		IConcurrentDictionary<DbDataReader, SqlTiming> _inProgressReaders = new MiniConcurrentDictionary<DbDataReader, SqlTiming>();
#else
		IConcurrentDictionary<Tuple<object, ExecuteType>, SqlTiming> _inProgress = new MiniConcurrentDictionary<Tuple<object, ExecuteType>, SqlTiming>();
		IConcurrentDictionary<DbDataReader, SqlTiming> _inProgressReaders = new MiniConcurrentDictionary<DbDataReader, SqlTiming>();
#endif
		/// <summary>
        /// The profiling session this SqlProfiler is part of.
        /// </summary>
        public MiniProfiler Profiler { get; private set; }

        /// <summary>
        /// Returns a new SqlProfiler to be used in the 'profiler' session.
        /// </summary>
        public SqlProfiler(MiniProfiler profiler)
        {
            Profiler = profiler;
        }

        /// <summary>
        /// Tracks when 'command' is started.
        /// </summary>
        public void ExecuteStartImpl(DbCommand command, ExecuteType type)
        {
            var id = Tuple.Create((object)command, type);
            var sqlTiming = new SqlTiming(command, type, Profiler);

            _inProgress[id] = sqlTiming;
        }
        /// <summary>
        /// Returns all currently open commands on this connection
        /// </summary>
        public SqlTiming[] GetInProgressCommands()
        {
            return _inProgress.Values.OrderBy(x => x.StartMilliseconds).ToArray();
        }
        /// <summary>
        /// Finishes profiling for 'command', recording durations.
        /// </summary>
        public void ExecuteFinishImpl(DbCommand command, ExecuteType type, DbDataReader reader = null)
        {
            var id = Tuple.Create((object)command, type);
            var current = _inProgress[id];
            current.ExecutionComplete(isReader: reader != null);
            SqlTiming ignore;
            _inProgress.TryRemove(id, out ignore);
            if (reader != null)
            {
                _inProgressReaders[reader] = current;
            }
        }

        /// <summary>
        /// Called when 'reader' finishes its iterations and is closed.
        /// </summary>
        public void ReaderFinishedImpl(DbDataReader reader)
        {
            SqlTiming stat;
            // this reader may have been disposed/closed by reader code, not by our using()
            if (_inProgressReaders.TryGetValue(reader, out stat))
            {
                stat.ReaderFetchComplete();
                SqlTiming ignore;
                _inProgressReaders.TryRemove(reader, out ignore);
            }
        }
    }

    /// <summary>
    /// Helper methods that allow operation on SqlProfilers, regardless of their instantiation.
    /// </summary>
    public static class SqlProfilerExtensions
    {
        /// <summary>
        /// Tracks when 'command' is started.
        /// </summary>
        public static void ExecuteStart(this SqlProfiler sqlProfiler, DbCommand command, ExecuteType type)
        {
            if (sqlProfiler == null) return;
            sqlProfiler.ExecuteStartImpl(command, type);
        }

        /// <summary>
        /// Finishes profiling for 'command', recording durations.
        /// </summary>
        public static void ExecuteFinish(this SqlProfiler sqlProfiler, DbCommand command, ExecuteType type, DbDataReader reader = null)
        {
            if (sqlProfiler == null) return;
            sqlProfiler.ExecuteFinishImpl(command, type, reader);
        }

        /// <summary>
        /// Called when 'reader' finishes its iterations and is closed.
        /// </summary>
        public static void ReaderFinish(this SqlProfiler sqlProfiler, DbDataReader reader)
        {
            if (sqlProfiler == null) return;
            sqlProfiler.ReaderFinishedImpl(reader);
        }

    }

	internal interface IConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>
	{
		bool TryRemove(TKey key, out TValue value);
	}

#if CSHARP30
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