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
    public partial class User
    {
        public string UserRoleDesc { get;set;}
        public string ThemeDesc { get; set; }
        public string LanguageDesc { get; set; }
        public string DepartmentDesc { get; set; }
        public string Fullname { get; set; }
        public string FarmerID { get; set; }
        public string SupplierID { get; set; }
    }
}
