﻿namespace StackExchange.Profiling.Wcf
{
    using System.Diagnostics;

    /// <summary>
    /// The profiler extensions.
    /// </summary>
    internal static class ProfilerExtensions
    {
        /// <summary>
        /// We don't actually know the start milliseconds, but lets 
        /// take it as zero being the start of the current head
        /// </summary>
        /// <param name="timing">the timing data</param>
        /// <param name="newStartMilliseconds">new Start Milliseconds.</param>
        public static void UpdateStartMillisecondTimingsToAbsolute(this Timing timing, decimal newStartMilliseconds)
        {
            if (timing == null)
                return;

            UpdateStartMillisecondTimingsByDelta(timing, newStartMilliseconds - timing.StartMilliseconds);
        }

        /// <summary>
        /// Delta is added to the existing StartMillisecondsValue
        /// Recursive method
        /// </summary>
        /// <param name="timing">The timing.</param>
        /// <param name="deltaMilliseconds">The delta Milliseconds.</param>
        public static void UpdateStartMillisecondTimingsByDelta(this Timing timing, decimal deltaMilliseconds)
        {
            if (timing == null)
                return;

            timing.StartMilliseconds += deltaMilliseconds;
            if (timing.Children != null)
            {
                foreach (var child in timing.Children)
                {
                    UpdateStartMillisecondTimingsByDelta(child, deltaMilliseconds);
                }
            }

            if (timing.SqlTimings != null)
            {
                foreach (var child in timing.SqlTimings)
                {
                    child.StartMilliseconds += deltaMilliseconds;                    
                }
            }
        }

        /// <summary>
        /// Removes trivial items from the current profiler results
        /// </summary>
        /// <param name="timing">The timing.</param>
        public static void RemoveTrivialTimings(this Timing timing)
        {
            if (timing.Children != null)
            {
                // This assumes that trivial items do not have any non-trivial children
                timing.Children.RemoveAll(child => child.IsTrivial);
            }

            Debug.Assert(timing.Children != null, "timing.Children != null");
            timing.Children.ForEach(child => child.RemoveTrivialTimings());
        }
    }
}
