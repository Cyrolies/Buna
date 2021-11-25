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
        public string UserRoleName { get;set;}
        //public string Supervisor
        //{
        //    get
        //    {
        //        return this.User2?.UserName;
        //    }
        //}
        //public string Language
        //{
        //    get
        //    {
        //        return this.StpData?.DataDescription;
        //    }
        //}
        //public string Theme
        //{
        //    get
        //    {
        //        return this.StpData1?.DataDescription;
        //    }
        //}
        //public string Department
        //{
        //    get
        //    {
        //        return this.StpData2?.DataDescription;
        //    }
        //}
        //public string OrganizationName
        //{
        //    get
        //    {
        //        return this.Organization1?.OrganizationName;
        //    }
        //}
        //public string Status
        //{
        //    get
        //    {
        //        return this.StcData?.Description;
        //    }
        //}
        //public string CreatedBy
        //{
        //    get
        //    {
        //        return this.User3?.UserName;
        //    }
        //}
    }
}
