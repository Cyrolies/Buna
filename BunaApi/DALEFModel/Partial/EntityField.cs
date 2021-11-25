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
    public partial class EntityField
    { 
        private string entityfieldname = string.Empty;
        private string entityFieldDataTypeDesc = string.Empty;
        private string stcControlTypeDesc = string.Empty;
        private string entityDesc = string.Empty;
        public string EntityAndFieldName
        {
            get
            {
                if (Entity != null)
                {
                    //this is only used when the releated entity is not found then we display all entityfields with there entity names
                    entityfieldname = this.Entity.Name + " " + this.EntityFieldName;
                }
                else
                {
                    entityfieldname = this.EntityFieldName;
                }
                return entityfieldname;
            }

        }

        public string EntityFieldDataTypeDesc
        {
            get
            {
                return entityFieldDataTypeDesc;
            }

            set
            {
                entityFieldDataTypeDesc = value;
            }
        }

        public string StcControlTypeDesc
        {
            get
            {
                return stcControlTypeDesc;
            }

            set
            {
                stcControlTypeDesc = value;
            }
        }

        public string EntityDesc
        {
            get
            {
                return entityDesc;
            }

            set
            {
                entityDesc = value;
            }
        }
    }
}
