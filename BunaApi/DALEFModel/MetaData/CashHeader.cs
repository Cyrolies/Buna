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
	[MetadataTypeAttribute(typeof(CashHeader.CashHeaderMetadata))]
	public partial class CashHeader
	{
		internal sealed class CashHeaderMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private CashHeaderMetadata()
			{
			}

			[StringLength(100,MinimumLength = 0,ErrorMessage = " ShipmentNumber Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "ShipmentNumber", Order = 1)]
			public string ShipmentNumber { get; set; }

			[StringLength(5,MinimumLength = 0,ErrorMessage = " Currency Maximum length is = 5 Minimum length is = 0")]
			[Display(Name = "Currency", Order = 2)]
			public string Currency { get; set; }

			[Display(Name = "Amount", Order = 3)]
			public decimal Amount { get; set; }

			[StringLength(2,MinimumLength = 0,ErrorMessage = " ColType Maximum length is = 2 Minimum length is = 0")]
			[Display(Name = "ColType", Order = 4)]
			public string ColType { get; set; }

			[Display(Name = "SyncedDateTime", Order = 5)]
			public DateTime SyncedDateTime { get; set; }

			[Key]
			[Display(Name = "Id", Order = 6)]
			public int Id { get; set; }

		}
	}
}
