﻿namespace StackExchange.Profiling
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlTypes;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using System.Web.Script.Serialization;

    using StackExchange.Profiling.Data;
    using StackExchange.Profiling.Helpers;

    /// <summary>
    /// Profiles a single SQL execution.
    /// </summary>
    [DataContract]
    public class SqlTiming
    {
        /// <summary>
        /// Holds the maximum size that will be stored for byte[] parameters
        /// </summary>
        private const int MaxByteParameterSize = 512;

        /// <summary>
        /// The profiler.
        /// </summary>
        private readonly MiniProfiler _profiler;

        /// <summary>
        /// The start ticks.
        /// </summary>
        private readonly long _startTicks;

        /// <summary>
        /// The _parent timing.
        /// </summary>
        private Timing _parentTiming;

        /// <summary>
        /// Initialises a new instance of the <see cref="SqlTiming"/> class. 
        /// Obsolete - used for serialization.
        /// </summary>
        [Obsolete("Used for serialization")]
        public SqlTiming()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SqlTiming"/> class. 
        /// Creates a new <c>SqlTiming</c> to profile 'command'.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="type">The type.</param>
        /// <param name="profiler">The profiler.</param>
        public SqlTiming(IDbCommand command, ExecuteType type, MiniProfiler profiler)
        {
            Id = Guid.NewGuid();

            CommandString = AddSpacesToParameters(command.CommandText);
            Parameters = GetCommandParameters(command);
            ExecuteType = type;

            if (!MiniProfiler.Settings.ExcludeStackTraceSnippetFromSqlTimings)
                StackTraceSnippet = Helpers.StackTraceSnippet.Get();

            _profiler = profiler;
            if (_profiler != null)
            {
                _profiler.AddSqlTiming(this);
                _startTicks = _profiler.ElapsedTicks;
                StartMilliseconds = _profiler.GetRoundedMilliseconds(_startTicks);
            }
        }

        /// <summary>
        /// Gets or sets a unique identifier for this <c>SqlTiming</c>
        /// </summary>
        [ScriptIgnore]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the category of SQL statement executed.
        /// </summary>
        [DataMember(Order = 1)]
        public ExecuteType ExecuteType { get; set; }

        /// <summary>
        /// Gets or sets the SQL that was executed.
        /// </summary>
        [ScriptIgnore]
        [DataMember(Order = 2)]
        public string CommandString { get; set; }

        /// <summary>
        /// Gets the command string with special formatting applied based on <c>MiniProfiler.Settings.SqlFormatter</c>
        /// </summary>
        public string FormattedCommandString
        {
            get
            {
                if (MiniProfiler.Settings.SqlFormatter == null) 
                    return CommandString;

                return MiniProfiler.Settings.SqlFormatter.FormatSql(this);
            }
        }

        /// <summary>
        /// Gets or sets roughly where in the calling code that this SQL was executed.
        /// </summary>
        [DataMember(Order = 3)]
        public string StackTraceSnippet { get; set; }

        /// <summary>
        /// Gets or sets the offset from main <c>MiniProfiler</c> start that this SQL began.
        /// </summary>
        [DataMember(Order = 4)]
        public decimal StartMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets how long this SQL statement took to execute.
        /// </summary>
        [DataMember(Order = 5)]
        public decimal DurationMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets When executing readers, how long it took to come back initially from the database, 
        /// before all records are fetched and reader is closed.
        /// </summary>
        [DataMember(Order = 6)]
        public decimal FirstFetchDurationMilliseconds { get; set; }

        /// <summary>
        /// Gets or sets any parameter names and values used by the profiled <c>DbCommand.</c>
        /// </summary>
        [DataMember(Order = 7)]
        public List<SqlTimingParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets Id of the Timing this statement was executed in.
        /// </summary>
        /// <remarks>
        /// Needed for database deserialization.
        /// </remarks>
        public Guid? ParentTimingId { get; set; }

        /// <summary>
        /// Gets or sets the Timing step that this SQL execution occurred in.
        /// </summary>
        [ScriptIgnore]
        public Timing ParentTiming
        {
            get
            {
                return _parentTiming;
            }

            set
            {
                _parentTiming = value;

                if (value != null && ParentTimingId != value.Id)
                    ParentTimingId = value.Id;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether other identical SQL statements have been executed during this MiniProfiler session.
        /// </summary>
        [DataMember(Order = 9)]
        public bool IsDuplicate { get; set; }

        /// <summary>
        /// Returns a snippet of the SQL command and the duration.
        /// </summary>
        /// <returns>the string representation</returns>
        public override string ToString()
        {
            return CommandString.Truncate(30) + " (" + DurationMilliseconds + " ms)";
        }

        /// <summary>
        /// Returns true if Ids match.
        /// </summary>
        /// <param name="rValue">
        /// The rValue.
        /// </param>
        /// <returns>true if rValue is equal to </returns>
        public override bool Equals(object rValue)
        {
            return rValue is SqlTiming && Id.Equals(((SqlTiming)rValue).Id);
        }

        /// <summary>
        /// Returns hash code of Id.
        /// </summary>
        /// <returns>the hash code value.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Called when command execution is finished to determine this <c>SqlTiming's</c> duration.
        /// </summary>
        /// <param name="isReader">The Reader.</param>
        public void ExecutionComplete(bool isReader)
        {
            if (isReader)
            {
                FirstFetchDurationMilliseconds = GetDurationMilliseconds();
            }
            else
            {
                DurationMilliseconds = GetDurationMilliseconds();
            }
        }

        /// <summary>
        /// Called when database reader is closed, ending profiling for <see cref="StackExchange.Profiling.Data.ExecuteType.Reader"/> <c>SqlTimings</c>.
        /// </summary>
        public void ReaderFetchComplete()
        {
            DurationMilliseconds = GetDurationMilliseconds();
        }

        /// <summary>
        /// Returns the value of <paramref name="parameter"/> suitable for storage/display.
        /// </summary>
        /// <param name="parameter">The DB Parameter.
        /// </param>
        /// <returns>a string containing the value.</returns>
        private static string GetValue(IDataParameter parameter)
        {
            object rawValue = parameter.Value;
            if (rawValue == null || rawValue == DBNull.Value)
            {
                return null;
            }

            // This assumes that all SQL variants use the same parameter format, it works for T-SQL
            if (parameter.DbType == DbType.Binary)
            {
                var bytes = rawValue as byte[];
                if (bytes != null && bytes.Length <= MaxByteParameterSize)
                {
                    return "0x" + BitConverter.ToString(bytes).Replace("-", string.Empty);
                }

                // Parameter is too long, so blank it instead
                return null;
            }

            if (rawValue is DateTime)
            {
                return ((DateTime)rawValue).ToString("s", System.Globalization.CultureInfo.InvariantCulture);
            }

            // we want the integral value of an enum, not its string representation
            var rawType = rawValue.GetType();
            if (rawType.IsEnum)
            {
                // use ChangeType, as we can't cast - http://msdn.microsoft.com/en-us/library/exx3b86w(v=vs.80).aspx
                return Convert.ChangeType(rawValue, Enum.GetUnderlyingType(rawType)).ToString();
            }

            return rawValue.ToString();
        }

        /// <summary>
        /// get the parameter size.
        /// </summary>
        /// <param name="parameter">The DB parameter.</param>
        /// <returns>the parameter size</returns>
        private static int GetParameterSize(IDbDataParameter parameter)
        {
            var value = parameter.Value as INullable;
            if (value != null)
            {
                var nullable = value;
                if (nullable.IsNull)
                {
                    return 0;
                }
            }

            return parameter.Size;
        }

        /// <summary>
        /// get the duration in milliseconds.
        /// </summary>
        /// <returns>return the duration in milliseconds</returns>
        private decimal GetDurationMilliseconds()
        {
            return _profiler.GetRoundedMilliseconds(_profiler.ElapsedTicks - _startTicks);
        }

        /// <summary>
        /// To help with display, put some space around <c>sammiched</c> commas
        /// </summary>
        /// <param name="commandString">The command String.</param>
        /// <returns>a string containing the formatted string.</returns>
        private string AddSpacesToParameters(string commandString)
        {
            return Regex.Replace(commandString, @",([^\s])", ", $1");
        }

        /// <summary>
        /// get the command parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>the list of SQL timing parameters</returns>
        private List<SqlTimingParameter> GetCommandParameters(IDbCommand command)
        {
            if (command.Parameters == null || command.Parameters.Count == 0) return null;

            var result = new List<SqlTimingParameter>();

            foreach (DbParameter parameter in command.Parameters)
            {
                if (!parameter.ParameterName.IsNullOrWhiteSpace())
                {
                    result.Add(new SqlTimingParameter
                    {
                        ParentSqlTimingId = Id,
                        Name = parameter.ParameterName.Trim(),
                        Value = GetValue(parameter),
                        DbType = parameter.DbType.ToString(),
                        Size = GetParameterSize(parameter)
                    });
                }
            }

            return result;
        }
    }
}