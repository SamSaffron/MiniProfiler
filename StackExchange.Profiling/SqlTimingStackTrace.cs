using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Runtime.Serialization;

namespace StackExchange.Profiling
{
    /// <summary>
    /// A stack trace entry
    /// </summary>
    [DataContract]
    public class SqlTimingStackTrace
    {
        /// <summary>
        /// The name of the method
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The method definition
        /// </summary>
        [DataMember]
        public string Definition { get; set; }

        /// <summary>
        /// The assembly containing the method
        /// </summary>
        [DataMember]
        public string Assembly { get; set; }

        /// <summary>
        /// The full path of the method
        /// </summary>
        [DataMember]
        public string FullPath { get; set; }

    }
}
