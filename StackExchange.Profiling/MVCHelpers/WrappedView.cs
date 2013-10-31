#if ASP_NET_MVC3
namespace StackExchange.Profiling.MVCHelpers
{
    using System.Web.Mvc;

    /// <summary>
    /// Wrapped MVC View that ProfilingViewEngine uses to log profiling data
    /// </summary>
    public class WrappedView : IView
    {
        /// <summary>
        /// Gets MVC IView that is wrapped by the ProfilingViewEngine.
        /// </summary>
        public IView Wrapped { get; private set; }

        /// <summary>
        /// Gets or sets the wrapped view name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the wrapped view is partial.
        /// </summary>
        public bool IsPartial { get; set; }

        /// <summary>
        /// Gets the wrapped view path.
        /// </summary>
        public string ViewPath
        {
            get
            {
                var view = Wrapped as RazorView;
                return view != null ? view.ViewPath : null;
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WrappedView"/> class. 
        /// </summary>
        /// <param name="wrapped">
        /// IView to wrap
        /// </param>
        /// <param name="name">
        /// Name/Path to view
        /// </param>
        /// <param name="isPartial">
        /// Whether view is Partial
        /// </param>
        public WrappedView(IView wrapped, string name, bool isPartial)
        {
            Wrapped = wrapped;
            Name = name;
            IsPartial = isPartial;
        }

        /// <summary>
        /// Renders the WrappedView and logs profiling data
        /// </summary>
        /// <param name="viewContext">
        /// The view Context.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public void Render(ViewContext viewContext, System.IO.TextWriter writer)
        {
            using (MiniProfiler.Current.Step("Render " + (IsPartial ? "partial" : string.Empty) + ": " + Name))
            {
                Wrapped.Render(viewContext, writer);
            }
        }
    }
}
#endif