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
	public partial class Person
	{

			public string Fullname{ get { return Firstnames+"  "+Surname;}}
			public string TitleDesc { get; set; }
			public string PersonCategoryDesc { get; set; }
			public string PersonTypeDesc { get; set; }
			public string OrgDesc { get; set; }
			public string CreatedByDesc { get; set; }
			public string ChangedByDesc { get; set; }
			public string StatusDesc { get; set; }
	}
}
