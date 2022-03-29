using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace DALEFModel
{

	/// <summary>
	/// Metadata class which has all the dataannotation attributes 
	/// Author: Robin Cyrolies
	/// </summary>
	[MetadataTypeAttribute(typeof(Person.PersonMetadata))]
	public partial class Person
	{
		internal sealed class PersonMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private PersonMetadata()
			{
			}

			[Display(Name = "Firstnames+'  '+Surname", Order = 1)]
			public string Fullname { get; set; }

			[Display(Name = "Gender", Order = 2)]
			public int StpGenderID { get; set; }

			[Display(Name = "Province", Order = 3)]
			public int StpProvinceID { get; set; }

			[Display(Name = "District", Order = 4)]
			public int StpDistrictID { get; set; }

			[Display(Name = "Constituency", Order = 5)]
			public int StpConstituencyID { get; set; }

			[Display(Name = "Ward", Order = 6)]
			public int StpWardID { get; set; }

			//[Display(Name = "Farm Type", Order = 7)]
			//public int StpFarmTypeID { get; set; }

			//[StringLength(50,MinimumLength = 0,ErrorMessage = " Age Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Age", Order = 8)]
			public int Age { get; set; }

			[Display(Name = "Correspondence Type", Order = 12)]
			public int StpCorrespondenceTypeID { get; set; }

			[Required(ErrorMessage = "Firstname is required")]
			[StringLength(50,MinimumLength = 0,ErrorMessage = " Firstnames Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Firstnames", Order = 13)]
			public string Firstnames { get; set; }

			[Required(ErrorMessage = "Surname is required")]
			[StringLength(50,MinimumLength = 0,ErrorMessage = " Surname Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Surname", Order = 14)]
			public string Surname { get; set; }

			[Required(ErrorMessage = "Title is required")]
			[Display(Name = "Title", Order = 15)]
			public int StpTitleID { get; set; }

			//[Display(Name = "Person Category", Order = 16)]
			//public int StpPersonCategoryID { get; set; }

			//[Display(Name = "Person Type", Order = 17)]
			//public int StpPersonTypeID { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Email Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Email", Order = 18)]
			public string Email { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Home Phone Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Home Phone", Order = 19)]
			public string HomePhone { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Mobile Phone Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Mobile Phone", Order = 20)]
			public string MobilePhone { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Work Phone Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Work Phone", Order = 21)]
			public string WorkPhone { get; set; }

			[Required(ErrorMessage = "Physical address is required")]
			[StringLength(500,MinimumLength = 0,ErrorMessage = " Physical Address Maximum length is = 500 Minimum length is = 0")]
			[Display(Name = "Physical Address", Order = 22)]
			public string PhysicalAddress { get; set; }

			[StringLength(500,MinimumLength = 0,ErrorMessage = " Postal Address Maximum length is = 500 Minimum length is = 0")]
			[Display(Name = "Postal Address", Order = 23)]
			public string PostalAddress { get; set; }

			[Display(Name = "Is Active", Order = 24)]
			public bool IsActive { get; set; }

			[Required(ErrorMessage = "Country is required")]
			[Display(Name = "Country", Order = 25)]
			public int OrgID { get; set; }

			[Display(Name = "Created By ", Order = 26)]
			public int CreatedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 27)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 28)]
			public DateTime ChangeDateTime { get; set; }

			[Display(Name = "Changed By", Order = 29)]
			public int ChangedByID { get; set; }

			[Display(Name = "hasSmartDevice", Order = 30)]
			public bool hasSmartDevice { get; set; }

			[Display(Name = "hasFinance", Order = 31)]
			public bool hasFinance { get; set; }

			[Key]
			[Display(Name = "Person", Order = 32)]
			public int PersonID { get; set; }

			

		}
	}
}
