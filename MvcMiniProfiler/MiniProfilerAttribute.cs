using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace MvcMiniProfiler
{
    /// <summary>
    /// Additional attributes that need to be persisted for this session
    /// </summary>
    [DataContract]
    public class MiniProfilerAttribute
    {
        ///<summary>
        /// Constructor for the MiniProfilerAttribute class
        ///</summary>
        public MiniProfilerAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
        /// <summary>
        /// Attribute name
        /// </summary>
        [DataMember(Order = 1)]
        public string Name { get; set; }

        /// <summary>
        /// The value submitted for the attribute
        /// </summary>
        [DataMember(Order = 2)]
        public string Value { get; set; }

    }
}
