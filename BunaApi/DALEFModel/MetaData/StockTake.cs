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
	[MetadataTypeAttribute(typeof(StockTake.StockTakeMetadata))]
	public partial class StockTake
	{
		internal sealed class StockTakeMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private StockTakeMetadata()
			{
			}

			[Required(ErrorMessage = " This field is required = Quantity")]
			[Display(Name = "Quantity", Order = 1)]
			public int Quantity { get; set; }

			[StringLength(150,MinimumLength = 0,ErrorMessage = " Location Maximum length is = 150 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Location")]
			[Display(Name = "Location", Order = 2)]
			public string Location { get; set; }

			[Required(ErrorMessage = " This field is required = StockTakeDate")]
			[Display(Name = "StockTakeDate", Order = 3)]
			public DateTime StockTakeDate { get; set; }

			[Display(Name = "CreatedBy", Order = 4)]
			public int CreatedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 5)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 6)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "StockTake", Order = 7)]
			public int StockTakeID { get; set; }

			[Required(ErrorMessage = " This field is required = Part")]
			[Display(Name = "Part", Order = 8)]
			public int PartID { get; set; }

		}
	}
}
