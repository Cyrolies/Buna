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
	[MetadataTypeAttribute(typeof(Consumable.ConsumableMetadata))]
	public partial class Consumable
	{
		internal sealed class ConsumableMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private ConsumableMetadata()
			{
			}

			[Display(Name = "Supplier", Order = 1)]
			public string Supplier { get; set; }

			[Display(Name = "Purchase Price", Order = 2)]
			public Decimal PurchasePrice { get; set; }

			[Display(Name = "Purchase Date", Order = 3)]
			public DateTime PurchaseDate { get; set; }

			[Display(Name = "Purchase Tax", Order = 4)]
			public Decimal PurchaseTax { get; set; }

			[Display(Name = "Purchase Costs", Order = 5)]
			public Decimal PurchaseCosts { get; set; }

			[Display(Name = "Is Purchase", Order = 6)]
			public bool IsPurchase { get; set; }

			[Display(Name = "Is Claimable", Order = 7)]
			public bool IsClaimable { get; set; }

			[Display(Name = "Created By", Order = 8)]
			public int CreatedByID { get; set; }

			[Display(Name = "Changed By", Order = 9)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 10)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 11)]
			public DateTime ChangeDateTime { get; set; }

			[Display(Name = "QuantityInUOM", Order = 12)]
			public Decimal QuantityInUOM { get; set; }

			[Display(Name = "Unit Of Measure For Open&Close", Order = 13)]
			public int StpUOMForOpenAndCloseID { get; set; }

			[Display(Name = "Opening Quantity", Order = 14)]
			public Decimal OpeningQuantity { get; set; }

			[Display(Name = "Closing Quantity", Order = 15)]
			public Decimal ClosingQuantity { get; set; }

			[StringLength(300,MinimumLength = 0,ErrorMessage = " Reason Maximum length is = 300 Minimum length is = 0")]
			[Display(Name = "Reason", Order = 16)]
			public string Reason { get; set; }

			[Display(Name = "Consumable", Order = 17)]
			public int StpConsumableTypeID { get; set; }

			[Display(Name = "Unit Of Measure", Order = 18)]
			public int StpUnitOfMeasureID { get; set; }

			[Display(Name = "From Asset", Order = 19)]
			public int AssetID { get; set; }

			[Key]
			[Display(Name = "Consumable", Order = 20)]
			public int ConsumableID { get; set; }

			[Display(Name = "Used On Asset", Order = 21)]
			public int UsedOnAssetID { get; set; }

		}
	}
}
