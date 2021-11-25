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
	[MetadataTypeAttribute(typeof(Part.PartMetadata))]
	public partial class Part
	{
		internal sealed class PartMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private PartMetadata()
			{
			}

			[Display(Name = "Barcode", Order = 1)]
			public string Barcode { get; set; }

			[Display(Name = "PartImage", Order = 2)]
			public string PartImage { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Colour Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Colour", Order = 3)]
			public string Colour { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Size Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Size", Order = 4)]
			public string Size { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Style Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Style", Order = 5)]
			public string Style { get; set; }

			[Display(Name = "Price", Order = 6)]
			public Decimal Price { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Supplier Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Supplier", Order = 7)]
			public string Supplier { get; set; }

			[Display(Name = "Warranty Expire Date", Order = 8)]
			public DateTime WarrantyExpireDate { get; set; }

			[Display(Name = "Is Active", Order = 9)]
			public bool IsActive { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Manufacturer Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Manufacturer", Order = 10)]
			public string Manufacturer { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Model Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Model", Order = 11)]
			public string Model { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Serial No Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Serial No", Order = 12)]
			public string SerialNo { get; set; }

			[Required(ErrorMessage = " This field is required = Code")]
			[Display(Name = "Code", Order = 13)]
			public string Code { get; set; }

			[StringLength(250,MinimumLength = 0,ErrorMessage = " Description Maximum length is = 250 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Description")]
			[Display(Name = "Description", Order = 14)]
			public string Description { get; set; }

			[Display(Name = "Created Date", Order = 15)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "Changed Date", Order = 16)]
			public DateTime ChangeDateTime { get; set; }

			[Required(ErrorMessage = " This field is required = Part Category")]
			[Display(Name = "Part Category", Order = 17)]
			public int StpPartCategoryID { get; set; }

			[Required(ErrorMessage = " This field is required = Unit Of Measure")]
			[Display(Name = "Unit Of Measure", Order = 18)]
			public int StpUnitOfMeasureID { get; set; }

			[Display(Name = "Created By", Order = 19)]
			public int CreatedByID { get; set; }

			[Key]
			[Display(Name = "Part", Order = 20)]
			public int PartID { get; set; }

		}
	}
}
