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

			

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Firstnames Maximum length is = 50 Minimum length is = ")]
			[Display(Name = "Firstnames", Order = 2)]
			public string Firstnames { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Surname Maximum length is = 50 Minimum length is = ")]
			[Display(Name = "Surname", Order = 3)]
			public string Surname { get; set; }

			[Display(Name = "Title", Order = 4)]
			public int StpTitleID { get; set; }

			[Display(Name = "PersonCategory", Order = 5)]
			public int StpPersonCategoryID { get; set; }

			[Display(Name = "PersonType", Order = 6)]
			public int StpPersonTypeID { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Email Maximum length is = 50 Minimum length is = ")]
			[Display(Name = "Email", Order = 7)]
			public string Email { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " HomePhone Maximum length is = 50 Minimum length is = ")]
			[Display(Name = "HomePhone", Order = 8)]
			public string HomePhone { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " MobilePhone Maximum length is = 50 Minimum length is = ")]
			[Display(Name = "MobilePhone", Order = 9)]
			public string MobilePhone { get; set; }

			[StringLength(50,MinimumLength =0 ,ErrorMessage = " WorkPhone Maximum length is = 50 Minimum length is = ")]
			[Display(Name = "WorkPhone", Order = 10)]
			public string WorkPhone { get; set; }

			[StringLength(500,MinimumLength = 0,ErrorMessage = " PhysicalAddress Maximum length is = 500 Minimum length is = ")]
			[Display(Name = "PhysicalAddress", Order = 11)]
			public string PhysicalAddress { get; set; }

			[StringLength(500,MinimumLength = 0,ErrorMessage = " PostalAddress Maximum length is = 500 Minimum length is = ")]
			[Display(Name = "PostalAddress", Order = 12)]
			public string PostalAddress { get; set; }

			[Display(Name = "CreatedBy", Order = 13)]
			public int CreatedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 14)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 15)]
			public DateTime ChangeDateTime { get; set; }

			[Display(Name = "ChangedBy", Order = 16)]
			public int ChangedByID { get; set; }

			[Key]
			[Display(Name = "Person", Order = 17)]
			public int PersonID { get; set; }

		}
	}
}
