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
    public partial class UserRoleActivity
    {
        private List<Activity> activities = new List<Activity>();
        private List<StcData> permissions = new List<StcData>();
        private List<UserRole> userroles = new List<UserRole>();

        public string ActivityDesc { get; set; }
        public string UserRoleDesc { get; set; }
        public string PermissionDesc { get; set; }
        public List<Activity> Activities
        {
            get
            {
                return activities;
            }

            set
            {
                activities = value;
            }
        }

        public List<StcData> Permissions
        {
            get
            {
                return permissions;
            }

            set
            {
                permissions = value;
            }
        }

        public List<UserRole> UserRoles
        {
            get
            {
                return userroles;
            }

            set
            {
                userroles = value;
            }
        }
    }
}

