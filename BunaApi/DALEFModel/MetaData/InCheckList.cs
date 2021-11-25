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
	[MetadataTypeAttribute(typeof(InCheckList.InCheckListMetadata))]
	public partial class InCheckList
	{
		internal sealed class InCheckListMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private InCheckListMetadata()
			{
			}

			[StringLength(400,MinimumLength = 0,ErrorMessage = " CheckItem Maximum length is = 400 Minimum length is = 0")]
			[Display(Name = "CheckItem", Order = 1)]
			public string CheckItem { get; set; }

			[Display(Name = "IsChecked", Order = 2)]
			public bool IsChecked { get; set; }

			[StringLength(300,MinimumLength = 0,ErrorMessage = " OverideReason Maximum length is = 300 Minimum length is = 0")]
			[Display(Name = "OverideReason", Order = 3)]
			public string OverideReason { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " OverideUser Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "OverideUser", Order = 4)]
			public string OverideUser { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " DriverNumber Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "DriverNumber", Order = 5)]
			public string DriverNumber { get; set; }

			[StringLength(150,MinimumLength = 0,ErrorMessage = " VehicleNumber Maximum length is = 150 Minimum length is = 0")]
			[Display(Name = "VehicleNumber", Order = 6)]
			public string VehicleNumber { get; set; }

			[StringLength(100,MinimumLength = 0,ErrorMessage = " ShipmentNumber Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "ShipmentNumber", Order = 7)]
			public string ShipmentNumber { get; set; }

			[Display(Name = "CreateDateTime", Order = 8)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 9)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "Id", Order = 10)]
			public int Id { get; set; }

		}
	}
}
