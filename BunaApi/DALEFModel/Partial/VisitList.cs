using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DALEFModel
{
    /// <summary>
    /// Partial class for VisitList model to add extra fields to model
    /// Author :Robin Cyrolies
    /// </summary>
    public partial class VisitList
    {
        public MobileUser Driver { get; set; }
        public string MonCheckBox { get; set; }
        public string TueCheckBox { get; set; }
        public string WedCheckBox { get; set; }
        public string ThuCheckBox { get; set; }
        public string FriCheckBox { get; set; }
        public string SatCheckBox { get; set; }
        public string SunCheckBox { get; set; }


    }
}
