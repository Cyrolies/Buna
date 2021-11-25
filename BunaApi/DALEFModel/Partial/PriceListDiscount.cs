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
    public partial class PriceListDiscount
    {
        public string DiscountDesc { get; set; }
        public int ProductID { get; set; }
        public string ProductDesc { get; set; }
        public int PriceListID { get; set; }
        public string PriceListDesc { get; set; }
        public string PriorityDesc { get; set; }
    }
}
