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
	[MetadataTypeAttribute(typeof(CashLine.CashLineMetadata))]
	public partial class CashLine
	{
		internal sealed class CashLineMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private CashLineMetadata()
			{
			}

			[StringLength(100,MinimumLength = 0,ErrorMessage = " ShipmentNumber Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "ShipmentNumber", Order = 1)]
			public string ShipmentNumber { get; set; }

			[StringLength(100,MinimumLength = 0,ErrorMessage = " OrderId Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "OrderId", Order = 2)]
			public string OrderId { get; set; }

			[StringLength(5,MinimumLength = 0,ErrorMessage = " Currency Maximum length is = 5 Minimum length is = 0")]
			[Display(Name = "Currency", Order = 3)]
			public string Currency { get; set; }

			[Display(Name = "Amount", Order = 4)]
			public decimal Amount { get; set; }

			[StringLength(2,MinimumLength = 0,ErrorMessage = " ColType Maximum length is = 2 Minimum length is = 0")]
			[Display(Name = "ColType", Order = 5)]
			public string ColType { get; set; }

			[StringLength(30,MinimumLength = 0,ErrorMessage = " Reference Maximum length is = 30 Minimum length is = 0")]
			[Display(Name = "Reference", Order = 6)]
			public string Reference { get; set; }

			[Display(Name = "SyncedDateTime", Order = 7)]
			public DateTime SyncedDateTime { get; set; }

			[Key]
			[Display(Name = "Id", Order = 8)]
			public int Id { get; set; }

		}
	}
}
