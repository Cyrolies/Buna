using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// Filter params
    /// </summary>
    public class FilterField
    {
        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public string Property { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public string Operator { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [exact match].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [exact match]; otherwise, <c>false</c>.
        /// </value>
       // public bool exactMatch { get; set; }
    }
}
