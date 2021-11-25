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
    public partial class StpData
    {
        private string stpDataTypeDesc;

        public string StpDataTypeDesc
        {
            get
            {
                return stpDataTypeDesc;
            }

            set
            {
                stpDataTypeDesc = value;
            }
        }
    }
}
