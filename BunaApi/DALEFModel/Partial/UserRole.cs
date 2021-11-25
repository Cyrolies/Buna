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
    public partial class UserRole
    {
        

        private List<User> users = new List<User>();

        public List<User> Users
        {
            get
            {
                return users;
            }

            set
            {
                users = value;
            }
        }
    }
}
