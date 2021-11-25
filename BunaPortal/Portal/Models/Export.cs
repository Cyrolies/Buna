using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Export
    {
        public string Entity { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        public bool CurrentPageOnly { get; set; } = false;
        public bool IncludeDetailsInExport { get; set; } = false;
        public bool Excel { get; set; } = true;
        public bool Pdf { get; set; } = false;
        public bool Csv { get; set; } = false;
        public bool ExportBlank { get; set; } = false;
        public bool ShowDBColumnNames { get; set; } = false;
        public string DatatableParams { get; set; } = string.Empty;
        public string HeaderDetail { get; set; } = string.Empty;
       // public List<T> dataList;
    }
     
    }