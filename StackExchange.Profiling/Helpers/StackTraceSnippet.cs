using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace StackExchange.Profiling.Helpers
{
	/// <summary>
	/// Gets part of a stack trace containing only methods we care about.
	/// </summary>
	public class StackTraceSnippet
	{
		private const string AspNetEntryPointMethodName = "System.Web.HttpApplication.IExecutionStep.Execute";

		/// <summary>
		/// Gets the current formatted and filted stack trace.
		/// </summary>
		/// <returns>Space separated list of methods</returns>
        public static List<SqlTimingStackTrace> Get()
		{
            var methods = new List<SqlTimingStackTrace>();
            var stringLength = 0;

            var frames = new StackTrace().GetFrames();
			if (frames == null)
			{
				return methods;
			}

			foreach (StackFrame t in frames)
			{
				var method = t.GetMethod();

				// no need to continue up the chain
				if (method.Name == AspNetEntryPointMethodName)
					break;

				var assembly = method.Module.Assembly.GetName().Name;
				if (!MiniProfiler.Settings.AssembliesToExclude.Contains(assembly) &&
					!ShouldExcludeType(method) &&
					!MiniProfiler.Settings.MethodsToExclude.Contains(method.Name))
				{
                    methods.Add(new SqlTimingStackTrace()
                    {
                        Name = method.Name,
                        Definition = method.ToString(),
                        FullPath = string.Format("{0}.{1}.{2}", method.DeclaringType.Namespace, method.DeclaringType.Name, method.Name),
                        Assembly = assembly,
                    });
                    stringLength += method.Name.Length;
				}

                // check against max length
                if (stringLength > MiniProfiler.Settings.StackMaxLength)
                    break;
			}

            return methods;
        }

		private static bool ShouldExcludeType(MethodBase method)
		{
			var t = method.DeclaringType;

			while (t != null)
			{
				if (MiniProfiler.Settings.TypesToExclude.Contains(t.Name))
					return true;

				t = t.DeclaringType;
			}
			return false;
		}
	}
}
