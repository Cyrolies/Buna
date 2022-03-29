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
	[MetadataTypeAttribute(typeof(Supplier.SupplierMetadata))]
	public partial class Supplier
	{
		internal sealed class SupplierMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private SupplierMetadata()
			{
			}

			[StringLength(1000,MinimumLength = 0,ErrorMessage = " Work Phone Maximum length is = 1000 Minimum length is = 0")]
			[Display(Name = "Work Phone", Order = 1)]
			public string WorkPhone { get; set; }

			[StringLength(1000,MinimumLength = 0,ErrorMessage = " Contact Fullname Maximum length is = 1000 Minimum length is = 0")]
			[Display(Name = "Contact Fullname", Order = 2)]
			public string ContactName { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Business Name Maximum length is = 50 Minimum length is = 0")]
			[Required(ErrorMessage = "Business Name")]
			[Display(Name = "Business Name", Order = 4)]
			public string Name { get; set; }

			[StringLength(350,MinimumLength = 0,ErrorMessage = " Supply following countries Maximum length is = 350 Minimum length is = 0")]
			[Display(Name = "Supply following countries", Order = 5)]
			public string Countries { get; set; }

			[StringLength(1000,MinimumLength = 0,ErrorMessage = " Description Maximum length is = 1000 Minimum length is = 0")]
			[Required(ErrorMessage = "Description")]
			[Display(Name = "Description", Order = 6)]
			public string Description { get; set; }

			[StringLength(100,MinimumLength = 0,ErrorMessage = " Email Maximum length is = 100 Minimum length is = 0")]
			[Required(ErrorMessage = "Email")]
			[Display(Name = "Email", Order = 7)]
			public string Email { get; set; }

			[StringLength(1000,MinimumLength = 0,ErrorMessage = " Mobile Phone Maximum length is = 1000 Minimum length is = 0")]
			[Required(ErrorMessage = "Mobile Phone")]
			[Display(Name = "Mobile Phone", Order = 8)]
			public string MobilePhone { get; set; }

			[StringLength(500,MinimumLength = 0,ErrorMessage = " PhysicalAddress Maximum length is = 500 Minimum length is = 0")]
			[Required(ErrorMessage = "Physical Address")]
			[Display(Name = "PhysicalAddress", Order = 9)]
			public string PhysicalAddress { get; set; }

			[StringLength(500,MinimumLength = 0,ErrorMessage = " PostalAddress Maximum length is = 500 Minimum length is = 0")]
			[Display(Name = "PostalAddress", Order = 10)]
			public string PostalAddress { get; set; }

			[Display(Name = "CreatedBy", Order = 11)]
			public int CreatedByID { get; set; }

			[Display(Name = "ChangedBy", Order = 12)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 13)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 14)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "Supplier", Order = 15)]
			public int SupplierID { get; set; }

		


		}
	}
}
