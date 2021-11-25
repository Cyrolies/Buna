using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DALEFModel
{
    /// <summary>
    /// Partial class for EntityField model to add extra field to model
    /// Author :Robin Cyrolies
    /// </summary>
    public partial class Entity
    {
        private string activityName = string.Empty;

        public string ActivityName
        {
            get
            {
                return activityName;
            }

            set
            {
                activityName = value;
            }
        }
    }
}
