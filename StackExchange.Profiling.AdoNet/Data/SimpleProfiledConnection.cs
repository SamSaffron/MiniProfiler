﻿namespace StackExchange.Profiling.Data
{
    using System;
    using System.Data;

    /// <summary>
    /// A general implementation of <c>IDbConnection</c> that uses an <see cref="IDbProfiler"/>
    /// to collect profiling information.
    /// </summary>
    public class SimpleProfiledConnection : IDbConnection
    {
        /// <summary>
        /// The profiler.
        /// </summary>
        private IDbProfiler _profiler;

        /// <summary>
        /// The connection.
        /// </summary>
        private IDbConnection _connection;

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString
        {
            get { return _connection.ConnectionString; }
            set { _connection.ConnectionString = value; }
        }

        /// <summary>
        /// Gets the connection timeout.
        /// </summary>
        public int ConnectionTimeout
        {
            get { return _connection.ConnectionTimeout; }
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        public string Database
        {
            get { return _connection.Database; }
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        public ConnectionState State
        {
            get { return _connection.State; }
        }

        /// <summary>
        /// Gets the internally wrapped <see cref="IDbConnection"/>
        /// </summary>
        public IDbConnection WrappedConnection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SimpleProfiledConnection"/> class. 
        /// Creates a simple profiled connection instance.
        /// </summary>
        /// <param name="connection">
        /// The database connection to wrap
        /// </param>
        /// <param name="profiler">
        /// The profiler to use
        /// </param>
        public SimpleProfiledConnection(IDbConnection connection, IDbProfiler profiler)
        {
            _connection = connection;
            if (profiler != null)
            {
                _profiler = profiler;
            }
        }

        /// <summary>
        /// begin the transaction.
        /// </summary>
        /// <returns>The <see cref="IDbTransaction"/>.</returns>
        public IDbTransaction BeginTransaction()
        {
            return new SimpleProfiledTransaction(_connection.BeginTransaction(), this);
        }

        /// <summary>
        /// begin a transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation Level.</param>
        /// <returns>the wrapped transaction</returns>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new SimpleProfiledTransaction(_connection.BeginTransaction(isolationLevel), this);
        }

        /// <summary>
        /// change the database.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public void ChangeDatabase(string databaseName)
        {
            _connection.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// create a new command.
        /// </summary>
        /// <returns>The <see cref="IDbCommand"/>.</returns>
        public IDbCommand CreateCommand()
        {
            return new SimpleProfiledCommand(_connection.CreateCommand(), this, _profiler);
        }

        /// <summary>
        /// close the connection
        /// </summary>
        public void Close()
        {
            _connection.Close();
        }

        /// <summary>
        /// open the connection
        /// </summary>
        public void Open()
        {
            _connection.Open();
        }

        /// <summary>
        /// dispose the command / connection and profiler.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// dispose the command / connection and profiler.
        /// </summary>
        /// <param name="disposing">false if the dispose is called from a <c>finalizer</c></param>
        private void Dispose(bool disposing)
        {
            if (disposing && _connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Dispose();
            }
            _connection = null;
            _profiler = null;
        }
    }
}
