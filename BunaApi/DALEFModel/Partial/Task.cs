using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DALEFModel
{
    /// <summary>
    /// Partial class for UserRoleActivity model to add extra field to model
    /// Author :Robin Cyrolies
    /// </summary>
    public partial class Task
    {
        private string resources;

        public string Resources
        {
            get { return resources; }
            set { resources = value; }
        }

    }
}


