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
	[MetadataTypeAttribute(typeof(InHelper.InHelperMetadata))]
	public partial class InHelper
	{
		internal sealed class InHelperMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private InHelperMetadata()
			{
			}

			[Key]
			[Display(Name = "Id", Order = 1)]
			public long Id { get; set; }

			//[StringLength(8,ErrorMessage = " PersonnelNumber Max length = 8")]
			//[Required(ErrorMessage = "PersonnelNumber")]
			[Display(Name = "PersonnelNumber", Order = 2)]
			public string PersonnelNumber { get; set; }

			//[StringLength(4,ErrorMessage = " CompanyCode Max length = 4")]
			//[Required(ErrorMessage = "CompanyCode")]
			[Display(Name = "CompanyCode", Order = 3)]
			public string CompanyCode { get; set; }

			//[StringLength(25,ErrorMessage = " FirstName Max length = 25")]
			//[Required(ErrorMessage = "FirstName")]
			[Display(Name = "FirstName", Order = 4)]
			public string FirstName { get; set; }

			//[StringLength(25,ErrorMessage = " LastName Max length = 25")]
			//[Required(ErrorMessage = "LastName")]
			[Display(Name = "LastName", Order = 5)]
			public string LastName { get; set; }

			//[StringLength(20,ErrorMessage = " IdNumber Max length = 20")]
			//[Required(ErrorMessage = "IdNumber")]
			[Display(Name = "IdNumber", Order = 6)]
			public string IdNumber { get; set; }

			[StringLength(4,ErrorMessage = " PersonnelArea Max length = 4")]
			[Display(Name = "PersonnelArea", Order = 7)]
			public string PersonnelArea { get; set; }

			[StringLength(4,ErrorMessage = " SubArea Max length = 4")]
			[Display(Name = "SubArea", Order = 8)]
			public string SubArea { get; set; }

			[StringLength(100,ErrorMessage = " ShipmentNumber Max length = 100")]
			[Required(ErrorMessage = "ShipmentNumber")]
			[Display(Name = "ShipmentNumber", Order = 9)]
			public string ShipmentNumber { get; set; }

			[Required(ErrorMessage = "CreateDateTime")]
			[Display(Name = "CreateDateTime", Order = 10)]
			public DateTime CreateDateTime { get; set; }

			[Required(ErrorMessage = "ChangeDateTime")]
			[Display(Name = "ChangeDateTime", Order = 11)]
			public DateTime ChangeDateTime { get; set; }

		}
	}
}
