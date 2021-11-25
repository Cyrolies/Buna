using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;


namespace DALEFModel
{

	[MetadataTypeAttribute(typeof(StcData.StcDataMetadata))]
	public partial class StcData
	{
		internal sealed class StcDataMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private StcDataMetadata()
			{
			}

			[Key]
			[Display(Name = "Data", Order = 1)]
			public int StcDataID { get; set; }

			[Required(ErrorMessage = "DataType")]
			[Display(Name = "DataType", Order = 2)]
			public int StcDataTypeID { get; set; }

			[StringLength(10,ErrorMessage = "Abbreviation")]
			[Display(Name = "Abbreviation", Order = 3)]
			public string Abbreviation { get; set; }

			[StringLength(50,ErrorMessage = "Description")]
			[Required(ErrorMessage = "Description")]
			[Display(Name = "Description", Order = 4)]
			public string Description { get; set; }

		}
	}
}
