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
	[MetadataTypeAttribute(typeof(CheckOut.CheckOutMetadata))]
	public partial class CheckOut
	{
		internal sealed class CheckOutMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private CheckOutMetadata()
			{
			}

			[StringLength(100,MinimumLength = 0,ErrorMessage = " ShipmentNumber Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "ShipmentNumber", Order = 1)]
			public string ShipmentNumber { get; set; }

			[StringLength(20,MinimumLength = 0,ErrorMessage = " CheckOutUser Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "CheckOutUser", Order = 2)]
			public string CheckOutUser { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " CheckOutWHSUser Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "CheckOutWHSUser", Order = 3)]
			public string CheckOutWHSUser { get; set; }

			[StringLength(20,MinimumLength = 0,ErrorMessage = " Odometer Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "Odometer", Order = 4)]
			public string Odometer { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " PlantDesc Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "PlantDesc", Order = 5)]
			public string PlantDesc { get; set; }

			[StringLength(18,MinimumLength = 0,ErrorMessage = " ProductId Maximum length is = 18 Minimum length is = 0")]
			[Display(Name = "ProductId", Order = 6)]
			public string ProductId { get; set; }

			[StringLength(100,MinimumLength = 0,ErrorMessage = " ProductDesc Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "ProductDesc", Order = 7)]
			public string ProductDesc { get; set; }

			[Display(Name = "UOM", Order = 8)]
			public string UOM { get; set; }

			[Display(Name = "CheckOutQty", Order = 9)]
			public int CheckOutQty { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " CheckerSignatureImageId Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "CheckerSignatureImageId", Order = 10)]
			public string CheckerSignatureImageId { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " DriverSignatureImageId Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "DriverSignatureImageId", Order = 11)]
			public string DriverSignatureImageId { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " CheckerWHSSignatureImageId Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "CheckerWHSSignatureImageId", Order = 12)]
			public string CheckerWHSSignatureImageId { get; set; }

			[Display(Name = "DateCreated", Order = 13)]
			public DateTime DateCreated { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " DriverNumber Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "DriverNumber", Order = 14)]
			public string DriverNumber { get; set; }

			[Display(Name = "SyncedDateTime", Order = 15)]
			public DateTime SyncedDateTime { get; set; }

			[Key]
			[Display(Name = "Id", Order = 16)]
			public int Id { get; set; }

		}
	}
}
