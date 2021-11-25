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
    public partial class SurveyQuestion
    {
        public string SurveyDesc { get; set; }
        public string IsImageDesc { get; set; }


    }
}
