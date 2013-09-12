﻿namespace StackExchange.Profiling
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An individual suppression block that deactivates profiling temporarily
    /// </summary>
    [DataContract]
    public class Suppression : IDisposable
    {
        private readonly bool _wasSuppressed;

        /// <summary>
        /// Initialises a new instance of the <see cref="Suppression"/> class. 
        /// Obsolete - used for serialization.
        /// </summary>
        [Obsolete("Used for serialization")]
        public Suppression()
        {

        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Suppression"/> class. 
        /// Creates a new Suppression to deactive profiling while alive
        /// </summary>
        /// <param name="profiler">
        /// The profiler.
        /// </param>
        public Suppression(MiniProfiler profiler)
        {
            if (profiler == null)
            {
                throw new ArgumentNullException("profiler");
            }

            Profiler = profiler;
            if (!Profiler.IsActive)
            {
                return;
            }

            Profiler.IsActive = false;
            _wasSuppressed = true;
        }

        /// <summary>
        /// Gets a reference to the containing profiler, allowing this Suppression to affect profiler activity.
        /// </summary>
        internal MiniProfiler Profiler { get; private set; }

        /// <summary>
        /// dispose the profiler.
        /// </summary>
        void IDisposable.Dispose()
        {
            if(Profiler != null && _wasSuppressed)
            {
                Profiler.IsActive = true;
            }
        }
    }
}