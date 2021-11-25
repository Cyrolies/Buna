using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DALEFModel
{
    /// <summary>
    /// Partial class for Activity model to add extra fields to model
    /// Author :Robin Cyrolies
    /// </summary>
    public partial class Discount
    {
        public string StpDiscountTypeDesc { get; set; }
        public string  StpConditionTypeDesc { get; set; }
        public string DiscountFullDesc { get; set; }
        public string DiscountDesc { get; set; }

    }
}
