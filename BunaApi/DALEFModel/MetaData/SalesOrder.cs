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
	[MetadataTypeAttribute(typeof(SalesOrder.SalesOrderMetadata))]
	public partial class SalesOrder
	{
		internal sealed class SalesOrderMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private SalesOrderMetadata()
			{
			}

			[StringLength(100,MinimumLength = 0,ErrorMessage = " OrderId Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "OrderId", Order = 1)]
			public string OrderId { get; set; }

			//[StringLength(10,MinimumLength = 0,ErrorMessage = " DeliveryNumber Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "DeliveryNumber", Order = 2)]
			public string DeliveryNumber { get; set; }

			//[StringLength(10,MinimumLength = 0,ErrorMessage = " SoldToParty Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "SoldToParty", Order = 3)]
			public string SoldToParty { get; set; }

			[StringLength(30,MinimumLength = 0,ErrorMessage = " SoldToName Maximum length is = 30 Minimum length is = 0")]
			[Display(Name = "SoldToName", Order = 4)]
			public string SoldToName { get; set; }

			//[StringLength(10,MinimumLength = 0,ErrorMessage = " ShipToParty Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "ShipToParty", Order = 5)]
			public string ShipToParty { get; set; }

			[StringLength(30,MinimumLength = 0,ErrorMessage = " ShipToName Maximum length is = 30 Minimum length is = 0")]
			[Display(Name = "ShipToName", Order = 6)]
			public string ShipToName { get; set; }

			[StringLength(20,MinimumLength = 0,ErrorMessage = " ReqDeliveryDate Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "ReqDeliveryDate", Order = 7)]
			public string ReqDeliveryDate { get; set; }

			[Display(Name = "Item", Order = 8)]
			public int Item { get; set; }

			//[StringLength(18,MinimumLength = 0,ErrorMessage = " ProductId Maximum length is = 18 Minimum length is = 0")]
			[Display(Name = "ProductId", Order = 9)]
			public string ProductId { get; set; }

			[Display(Name = "OrderQty", Order = 10)]
			public decimal OrderQty { get; set; }

			[Display(Name = "ConfirmQty", Order = 11)]
			public decimal ConfirmQty { get; set; }

			[Display(Name = "DeliveryQty", Order = 12)]
			public decimal DeliveryQty { get; set; }

			[StringLength(4,MinimumLength = 0,ErrorMessage = " ItemCategory Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "ItemCategory", Order = 13)]
			public string ItemCategory { get; set; }

			//[StringLength(3,MinimumLength = 0,ErrorMessage = " UOM Maximum length is = 3 Minimum length is = 0")]
			[Display(Name = "UOM", Order = 14)]
			public string UOM { get; set; }

			//[StringLength(4,MinimumLength = 0,ErrorMessage = " ShippingPoint Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "ShippingPoint", Order = 15)]
			public string ShippingPoint { get; set; }

			//[StringLength(6,MinimumLength = 0,ErrorMessage = " Route Maximum length is = 6 Minimum length is = 0")]
			[Display(Name = "Route", Order = 16)]
			public string Route { get; set; }

			[StringLength(4,MinimumLength = 0,ErrorMessage = " TermsOfPayment Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "TermsOfPayment", Order = 17)]
			public string TermsOfPayment { get; set; }

			[Display(Name = "ReturnQty", Order = 18)]
			public decimal ReturnQty { get; set; }

			[StringLength(1,MinimumLength = 0,ErrorMessage = " ReturnFlag Maximum length is = 1 Minimum length is = 0")]
			[Display(Name = "ReturnFlag", Order = 19)]
			public string ReturnFlag { get; set; }

			[StringLength(2,MinimumLength = 0,ErrorMessage = " ReturnReason Maximum length is = 2 Minimum length is = 0")]
			[Display(Name = "ReturnReason", Order = 20)]
			public string ReturnReason { get; set; }

			[StringLength(100,MinimumLength = 0,ErrorMessage = " ShipmentNumber Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "ShipmentNumber", Order = 21)]
			public string ShipmentNumber { get; set; }

			//[StringLength(10,MinimumLength = 0,ErrorMessage = " InvNumber Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "InvNumber", Order = 22)]
			public string InvNumber { get; set; }

			[Display(Name = "InvPosition", Order = 23)]
			public int InvPosition { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " StartTime Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "StartTime", Order = 24)]
			public string StartTime { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " EndTime Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "EndTime", Order = 25)]
			public string EndTime { get; set; }

			[Key]
			[Display(Name = "id", Order = 26)]
			public int id { get; set; }

		}
	}
}
