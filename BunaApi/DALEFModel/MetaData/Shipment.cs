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
	[MetadataTypeAttribute(typeof(Shipment.ShipmentMetadata))]
	public partial class Shipment
	{
		internal sealed class ShipmentMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private ShipmentMetadata()
			{
			}

			[StringLength(20,MinimumLength = 0,ErrorMessage = " DeliveryNumber Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "DeliveryNumber", Order = 1)]
			public string DeliveryNumber { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " DriverNumber Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "DriverNumber", Order = 2)]
			public string DriverNumber { get; set; }

			[StringLength(30,MinimumLength = 0,ErrorMessage = " DriverName Maximum length is = 30 Minimum length is = 0")]
			[Display(Name = "DriverName", Order = 3)]
			public string DriverName { get; set; }

			[StringLength(18,MinimumLength = 0,ErrorMessage = " VehicleNumber Maximum length is = 18 Minimum length is = 0")]
			[Display(Name = "VehicleNumber", Order = 4)]
			public string VehicleNumber { get; set; }

			[StringLength(40,MinimumLength = 0,ErrorMessage = " VehicleDesc Maximum length is = 40 Minimum length is = 0")]
			[Display(Name = "VehicleDesc", Order = 5)]
			public string VehicleDesc { get; set; }

			[StringLength(1,MinimumLength = 0,ErrorMessage = " SAPStatusNumber Maximum length is = 1 Minimum length is = 0")]
			[Display(Name = "SAPStatusNumber", Order = 6)]
			public string SAPStatusNumber { get; set; }

			[StringLength(40,MinimumLength = 0,ErrorMessage = " SAPStatusDesc Maximum length is = 40 Minimum length is = 0")]
			[Display(Name = "SAPStatusDesc", Order = 7)]
			public string SAPStatusDesc { get; set; }

			[StringLength(4,MinimumLength = 0,ErrorMessage = " ShipmentType Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "ShipmentType", Order = 8)]
			public string ShipmentType { get; set; }

			[StringLength(4,MinimumLength = 0,ErrorMessage = " TransportPlanPoint Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "TransportPlanPoint", Order = 9)]
			public string TransportPlanPoint { get; set; }

			[StringLength(20,MinimumLength = 0,ErrorMessage = " TransportPlanPointDesc Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "TransportPlanPointDesc", Order = 10)]
			public string TransportPlanPointDesc { get; set; }

			//[StringLength(50,MinimumLength = 0,ErrorMessage = " Route Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Route", Order = 11)]
			public string Route { get; set; }

			[StringLength(40,MinimumLength = 0,ErrorMessage = " RouteDesc Maximum length is = 40 Minimum length is = 0")]
			[Display(Name = "RouteDesc", Order = 12)]
			public string RouteDesc { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " ShippingPoint Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "ShippingPoint", Order = 13)]
			public string ShippingPoint { get; set; }

			[StringLength(4,MinimumLength = 0,ErrorMessage = " SalesOrg Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "SalesOrg", Order = 14)]
			public string SalesOrg { get; set; }

			[StringLength(4,MinimumLength = 0,ErrorMessage = " SalesOffice Maximum length is = 4 Minimum length is = 0")]
			[Display(Name = "SalesOffice", Order = 15)]
			public string SalesOffice { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " DSDStatusDesc Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "DSDStatusDesc", Order = 16)]
			public string DSDStatusDesc { get; set; }

			[Display(Name = "CreateDateTime", Order = 17)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 18)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "Id", Order = 19)]
			public int Id { get; set; }

			[StringLength(100,MinimumLength = 0,ErrorMessage = " ShipmentNumber Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "ShipmentNumber", Order = 20)]
			public string ShipmentNumber { get; set; }

		}
	}
}
