using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DALEFModel
{
    [Serializable]
    /// <summary>
    /// This is a base class for all the models The fields below are generic to every model 
    /// Author:Robin Cyrolies
    /// </summary>
    public class BaseClass
    {


       
        public bool IsActive { get; set; }
        public byte[] VersionNo { get; set; }
     
        public User CreatedByUser { get; set; }
        public User ChangedByUser { get; set; }

        public DateTime? CreateDateTime { get; set; } = DateTime.Now;
        public DateTime? ChangeDateTime { get; set; } = DateTime.Now;

        public string CreateDateTimeDisplay { get; set; }
        public string ChangeDateTimeDisplay { get; set; }

        # region Grid Html fields

		public string IsActiveCheckBox { get; set; }
        public string EditButton { get; set; }
        public string DeleteButton { get; set; }

        public int TotalRows { get; set; }

        #endregion

        #region Organization

        public int? OrgID { get; set; }


        #endregion

        #region Supervision
        public Nullable<int> StcStatusID { get; set; }
        public bool setToPendingUsable { get; set; }
      
        #endregion
    }
}
