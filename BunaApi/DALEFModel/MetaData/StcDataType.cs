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
	[MetadataTypeAttribute(typeof(StcDataType.StcDataTypeMetadata))]
	public partial class StcDataType
	{
		internal sealed class StcDataTypeMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private StcDataTypeMetadata()
			{
			}

			[Key]
			[Display(Name = "DataType", Order = 1)]
			public int StcDataTypeID { get; set; }

			[StringLength(50,ErrorMessage = "StaticDataType")]
			[Required(ErrorMessage = "StaticDataType")]
			[Display(Name = "StaticDataType", Order = 2)]
			public string StaticDataType { get; set; }

			[StringLength(20,ErrorMessage = "DataSection")]
			[Display(Name = "DataSection", Order = 3)]
			public string DataSection { get; set; }

		}
	}
}
