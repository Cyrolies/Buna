using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALEFModel
{
    public class ExportDataControl<T>
    {
        private string entity = string.Empty;
        private string controller = string.Empty;
        private string filename = string.Empty;
        private bool currentPageOnly = false;
        private bool includeDetailsInExport = false;
        private bool excel = true;
        private bool pdf = false;
        private bool csv = false;
        private bool exportBlank = false;
        private bool showDBColumnNames = false;
        private string datatableParams = string.Empty;
        private string headerDetail = string.Empty;
        private List<T> dataList;

        public string Filename
        {
            get
            {
                return filename;
            }

            set
            {
                filename = value;
            }
        }

        public bool CurrentPageOnly
        {
            get
            {
                return currentPageOnly;
            }

            set
            {
                currentPageOnly = value;
            }
        }

        public bool IncludeDetailsInExport
        {
            get
            {
                return includeDetailsInExport;
            }

            set
            {
                includeDetailsInExport = value;
            }
        }

        public bool Excel
        {
            get
            {
                return excel;
            }

            set
            {
                excel = value;
            }
        }

        public bool Pdf
        {
            get
            {
                return pdf;
            }

            set
            {
                pdf = value;
            }
        }

        public bool Csv
        {
            get
            {
                return csv;
            }

            set
            {
                csv = value;
            }
        }

        public bool ExportBlank
        {
            get
            {
                return exportBlank;
            }

            set
            {
                exportBlank = value;
            }
        }

        public string Entity
        {
            get
            {
                return entity;
            }

            set
            {
                entity = value;
            }
        }

        public string Controller
        {
            get
            {
                return controller;
            }

            set
            {
                controller = value;
            }
        }

        public bool ShowDBColumnNames
        {
            get
            {
                return showDBColumnNames;
            }

            set
            {
                showDBColumnNames = value;
            }
        }

        public List<T> DataList
        {
            get
            {
                return dataList;
            }

            set
            {
                dataList = value;
            }
        }

        public string DatatableParams
        {
            get
            {
                return datatableParams;
            }

            set
            {
                datatableParams = value;
            }
        }

        public string HeaderDetail
        {
            get
            {
                return headerDetail;
            }

            set
            {
                headerDetail = value;
            }
        }
    }
}
