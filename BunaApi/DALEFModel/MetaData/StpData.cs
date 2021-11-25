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
	[MetadataTypeAttribute(typeof(StpData.StpDataMetadata))]
	public partial class StpData
	{
		internal sealed class StpDataMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private StpDataMetadata()
			{
			}

			[Required(ErrorMessage = "Data Code")]
			[Display(Name = "Data Code", Order = 1)]
			public string  DataCode { get; set; }

			[Required(ErrorMessage = "Description")]
			[Display(Name = "Description", Order = 2)]
			public string DataDescription { get; set; }

			[Required(ErrorMessage = "Is Active")]
			[Display(Name = "Is Active", Order = 3)]
			public bool IsActive { get; set; }

			[Key]
			[Display(Name = "Data", Order = 6)]
			public string StpDataID { get; set; }

			[Required(ErrorMessage = "Data Type")]
			[Display(Name = "Data Type", Order = 7)]
			public int  StpDataTypeID { get; set; }

		}
	}
}
