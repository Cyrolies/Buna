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
	[MetadataTypeAttribute(typeof(Delivery.DeliveryMetadata))]
	public partial class Delivery
	{
		internal sealed class DeliveryMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private DeliveryMetadata()
			{
			}

			[Key]
			[Display(Name = "Id", Order = 1)]
			public int Id { get; set; }

			//[DecimalLength(60,MinimumLength = 0,ErrorMessage = " SAPGPSLongitude Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "SAPGPSLongitude", Order = 2)]
			public decimal SAPGPSLongitude { get; set; }

			//[StringLength(60,MinimumLength = 0,ErrorMessage = " SAPGPSLatitude Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "SAPGPSLatitude", Order = 3)]
			public decimal SAPGPSLatitude { get; set; }

			//[StringLength(50,MinimumLength = 0,ErrorMessage = " DeviceGPSLongitude Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "DeviceGPSLongitude", Order = 4)]
			public decimal DeviceGPSLongitude { get; set; }

			//[StringLength(50,MinimumLength = 0,ErrorMessage = " DeviceGPSLatitude Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "DeviceGPSLatitude", Order = 5)]
			public decimal DeviceGPSLatitude { get; set; }

			//[StringLength(60,MinimumLength = 0,ErrorMessage = " LatitudeDifference Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "LatitudeDifference", Order = 6)]
			public decimal LatitudeDifference { get; set; }

			//[StringLength(60,MinimumLength = 0,ErrorMessage = " LongitudeDifference Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "LongitudeDifference", Order = 7)]
			public decimal LongitudeDifference { get; set; }

			[StringLength(100,MinimumLength = 0,ErrorMessage = " ShipmentNumber Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "ShipmentNumber", Order = 8)]
			public string ShipmentNumber { get; set; }

			[StringLength(100,MinimumLength = 0,ErrorMessage = " OrderId Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "OrderId", Order = 9)]
			public string OrderId { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " DeliveryNumber Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "DeliveryNumber", Order = 10)]
			public string DeliveryNumber { get; set; }

			[Display(Name = "Item", Order = 11)]
			public int Item { get; set; }

			[StringLength(18,MinimumLength = 0,ErrorMessage = " ProductId Maximum length is = 18 Minimum length is = 0")]
			[Display(Name = "ProductId", Order = 12)]
			public string ProductId { get; set; }

			[StringLength(20,MinimumLength = 0,ErrorMessage = " DeliveryDate Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "DeliveryDate", Order = 13)]
			public string DeliveryDate { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " ShippingPoint Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "ShippingPoint", Order = 14)]
			public string ShippingPoint { get; set; }

			//[StringLength(50,MinimumLength = 0,ErrorMessage = " Route Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Route", Order = 15)]
			public string Route { get; set; }

			[StringLength(4,MinimumLength = 0,ErrorMessage = " SalesOrg Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "SalesOrg", Order = 16)]
			public string SalesOrg { get; set; }

			[StringLength(4,MinimumLength = 0,ErrorMessage = " DeliveryType Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "DeliveryType", Order = 17)]
			public string DeliveryType { get; set; }

			//[StringLength(10,MinimumLength = 0,ErrorMessage = " SoldTo Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "SoldTo", Order = 18)]
			public string SoldTo { get; set; }

			//[StringLength(30,MinimumLength = 0,ErrorMessage = " SoldToName Maximum length is = 30 Minimum length is = 0")]
			[Display(Name = "SoldToName", Order = 19)]
			public string SoldToName { get; set; }

			//[StringLength(10,MinimumLength = 0,ErrorMessage = " ShipTo Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "ShipTo", Order = 20)]
			public string ShipTo { get; set; }

			//[StringLength(50,MinimumLength = 0,ErrorMessage = " ShipToName Maximum length is = 30 Minimum length is = 0")]
			[Display(Name = "ShipToName", Order = 21)]
			public string ShipToName { get; set; }

			[Display(Name = "DeliveryQTY", Order = 22)]
			public decimal DeliveryQTY { get; set; }

			[Display(Name = "ConfirmedQTY", Order = 23)]
			public decimal ConfirmedQTY { get; set; }

			[StringLength(4,MinimumLength = 0,ErrorMessage = " ItemCategory Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "ItemCategory", Order = 24)]
			public string ItemCategory { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " ReturnReason Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "ReturnReason", Order = 25)]
			public string ReturnReason { get; set; }

			[StringLength(35,MinimumLength = 0,ErrorMessage = " PurchaseOrderNo Maximum length is = 35 Minimum length is = 0")]
			[Display(Name = "PurchaseOrderNo", Order = 26)]
			public string PurchaseOrderNo { get; set; }

			[Display(Name = "PurchaseOrderDate", Order = 27)]
			public DateTime PurchaseOrderDate { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " DriverImageId Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "DriverImageId", Order = 28)]
			public string DriverImageId { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " CustomerImageId Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "CustomerImageId", Order = 29)]
			public string CustomerImageId { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " FeedbackComment Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "FeedbackComment", Order = 30)]
			public string FeedbackComment { get; set; }

		}
	}
}
