using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Common;

namespace DALEFModel
{

	/// <summary>
	/// Partial class 
	/// Author: Robin Cyrolies
	/// </summary>
	public partial class Document
	{

			public string DocumentGroupDesc { get; set; }
			public string OrgDesc { get; set; }
			public string StatusDesc { get; set; }
			public string CreatedByDesc { get; set; }
			public string ChangedByDesc { get; set; }
			public HttpPostedFileBase postedFile { get; set; }
			public string DownloadButton { get; set; }
	}
}
