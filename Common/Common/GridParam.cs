using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GridParam
    {
        List<FilterField> listOrderBy = new List<FilterField>();

        /// <summary>
        /// Gets or sets the list order by.
        /// </summary>
        /// <value>
        /// The list order by.
        /// </value>
        public List<FilterField> ListOrderBy
        {
            get { return listOrderBy; }
            set { listOrderBy = value; }
        }
        List<FilterField> listFilterBy = new List<FilterField>();
        /// <summary>
        /// Gets or sets the list filter by.
        /// </summary>
        /// <value>
        /// The list filter by.
        /// </value>
        public List<FilterField> ListFilterBy
        {
            get { return listFilterBy; }
            set { listFilterBy = value; }
        }

        
        int pageNo = 0;

        /// <summary>
        /// Gets or sets the page no.
        /// </summary>
        /// <value>
        /// The page no.
        /// </value>
        public int PageNo
        {
            get { return pageNo; }
            set { pageNo = value; }
        }
        int pagesize = 0;

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GridParam"/> is includerelations.
        /// </summary>
        /// <value>
        ///   <c>true</c> if includerelations; otherwise, <c>false</c>.
        /// </value>
        public bool Includerelations
        {
            get
            {
                return includerelations;
            }

            set
            {
                includerelations = value;
            }
        }

        bool includerelations = false;

        public bool IncludeForeignKeyValues
        {
            get
            {
                return includeForeignKeyValues;
            }

            set
            {
                includeForeignKeyValues = value;
            }
        }

        public string EntityName
        {
            get
            {
                return entityName;
            }

            set
            {
                entityName = value;
            }
        }

        public int DatabaseToUse
        {
            get
            {
                return databaseToUse;
            }

            set
            {
                databaseToUse = value;
            }
        }

        public Type Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        bool includeForeignKeyValues = false;

        private string entityName = string.Empty;

        private int databaseToUse = 0;

        private Type type;
    }
}
