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
	[MetadataTypeAttribute(typeof(Asset.AssetMetadata))]
	public partial class Asset
	{
		internal sealed class AssetMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private AssetMetadata()
			{
			}

			[Key]
			[Display(Name = "Asset", Order = 1)]
			[Required(ErrorMessage = "Farm is required")]
			public int AssetID { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " Code Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "Code", Order = 2)]
			public string Code { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Name Maximum length is = 50 Minimum length is = 0")]
			[Required(ErrorMessage ="Farm name is required")]
			[Display(Name = "Name", Order = 3)]
			public string Name { get; set; }

			[StringLength(250,MinimumLength = 0,ErrorMessage = " Description Maximum length is = 250 Minimum length is = 0")]
			[Required(ErrorMessage = "Quantity is required")]
			[Display(Name = "Description", Order = 4)]
			public string Description { get; set; }

			[Display(Name = "PurchasePrice", Order = 5)]
			public Decimal PurchasePrice { get; set; }

			[Display(Name = "PurchaseDate", Order = 6)]
			public DateTime PurchaseDate { get; set; }

			[Display(Name = "PurchaseTax", Order = 7)]
			public Decimal PurchaseTax { get; set; }

			[Display(Name = "PurchaseCosts", Order = 8)]
			public Decimal PurchaseCosts { get; set; }

			[Display(Name = "AssetCategory", Order = 9)]
			[Required(ErrorMessage = "Farm type is required")]
			public int StpAssetCategoryID { get; set; }

			[Display(Name = "ParentAsset", Order = 10)]
			public int ParentAssetID { get; set; }

			[Display(Name = "PersonID", Order = 11)]
			[Required(ErrorMessage = "Farmer is required")]
			[Range(0, int.MaxValue, ErrorMessage = "Please select a Farmer")]
			public int PersonID { get; set; }

			[Display(Name = "DateAssigned", Order = 12)]
			public DateTime DateAssigned { get; set; }

			[Display(Name = "Barcode", Order = 13)]
			public byte[] Barcode { get; set; }

			[Display(Name = "SellPrice", Order = 14)]
			public Decimal SellPrice { get; set; }

			[Display(Name = "IsSold", Order = 15)]
			public bool IsSold { get; set; }

			[Display(Name = "CreatedBy", Order = 16)]
			public int CreatedByID { get; set; }

			[Display(Name = "ChangedBy", Order = 17)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 18)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 19)]
			public DateTime ChangeDateTime { get; set; }

			[Display(Name = "Latitude", Order = 20)]
			//[Required(ErrorMessage = "Latitude is required")]
			public decimal? Latitude { get; set; }

			[Display(Name = "Longitude", Order = 21)]
			//[Required(ErrorMessage = "Longitude is required")]
			public decimal? Longitude { get; set; }

			[Display(Name = "Size", Order = 22)]
			[Required(ErrorMessage = "Volume is required")]
			public string Size { get; set; }

		}
	}
}
