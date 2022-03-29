using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace DALEFModel
{

	/// <summary>
	/// Partial class 
	/// Author: Robin Cyrolies
	/// </summary>
	public partial class Product
	{

			public string OrgDesc { get; set; }
			public string StatusDesc { get; set; }
			public string CreatedByDesc { get; set; }
			public string ChangedByDesc { get; set; }
			public string CurrencyDesc { get; set; }
			public string ImageDesc { get; set; }
			public string SupplierDesc { get; set; }
			public System.Web.HttpPostedFileBase postedFile { get; set; }
	}
}
