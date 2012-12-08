using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using StackExchange.Profiling.Helpers;

namespace StackExchange.Profiling
{
    /// <summary>
    /// HttpContext based profiler provider.  This is the default provider to use in a web context.
    /// The current profiler is associated with a HttpContext.Current ensuring that profilers are 
    /// specific to a individual HttpRequest.
    /// </summary>
    public partial class WebRequestProfilerProvider40 : WebRequestProfilerProvider
    {
        /// <summary>
        /// Makes sure 'profiler' has a Name, pulling it from route data or url.
        /// </summary>
        private static void EnsureName(MiniProfiler profiler, HttpRequest request)
        {
            // also set the profiler name to Controller/Action or /url
            if (string.IsNullOrWhiteSpace(profiler.Name))
            {
                var rc = request.RequestContext;
                RouteValueDictionary values;

                if (rc != null && rc.RouteData != null && (values = rc.RouteData.Values).Count > 0)
                {
                    var controller = values["Controller"];
                    var action = values["Action"];

                    if (controller != null && action != null)
                        profiler.Name = controller.ToString() + "/" + action.ToString();
                }

                if (string.IsNullOrWhiteSpace(profiler.Name))
                {
                    profiler.Name = request.Url.AbsolutePath ?? "";
                    if (profiler.Name.Length > 50)
                        profiler.Name = profiler.Name.Remove(50);
                }
            }
        }

        /// <summary>
        /// Ends the current profiling session, if one exists.
        /// </summary>
        /// <param name="discardResults">
        /// When true, clears the <see cref="MiniProfiler.Current"/> for this HttpContext, allowing profiling to 
        /// be prematurely stopped and discarded. Useful for when a specific route does not need to be profiled.
        /// </param>
        public override void Stop(bool discardResults)
        {
            Stop(discardResults, EnsureName);
        }
    }
}