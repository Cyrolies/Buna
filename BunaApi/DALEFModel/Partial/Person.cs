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

			public string Fullname{ get { return Firstnames+" "+Surname;}}
			public string GenderDesc { get; set; }
			public string ProvinceDesc { get; set; }
			public string DistrictDesc { get; set; }
			public string ConstituencyDesc { get; set; }
			public string WardDesc { get; set; }
			public string FarmTypeDesc { get; set; }
			public string CorrespondenceTypeDesc { get; set; }
			public string TitleDesc { get; set; }
			public string PersonCategoryDesc { get; set; }
			public string PersonTypeDesc { get; set; }
			public string OrgDesc { get; set; }
			public string CreatedByDesc { get; set; }
			public string ChangedByDesc { get; set; }
			public string StatusDesc { get; set; }
			public string Farmname { get; set;}
			public int StpFarmTypeID { get; set; }
			public string Quantity { get; set; }
			public string Volume { get; set; }
			public string Longitude { get; set; }
			public string Latitude { get; set; }


	}
}
