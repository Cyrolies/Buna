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
	[MetadataTypeAttribute(typeof(MobileCheckList.MobileCheckListMetadata))]
	public partial class MobileCheckList
	{
		internal sealed class MobileCheckListMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private MobileCheckListMetadata()
			{
			}

			[Key]
			[Display(Name = "Id", Order = 1)]
			public int Id { get; set; }

			[StringLength(400,ErrorMessage = " CheckItem Max length = 400")]
			[Required(ErrorMessage = "CheckItem")]
			[Display(Name = "CheckItem", Order = 2)]
			public string CheckItem { get; set; }

			[Required(ErrorMessage = "IsMandatory")]
			[Display(Name = "IsMandatory", Order = 3)]
			public bool IsMandatory { get; set; }

			[Required(ErrorMessage = "IsOveridable")]
			[Display(Name = "IsOveridable", Order = 4)]
			public bool IsOveridable { get; set; }

			[Required(ErrorMessage = "CreateDateTime")]
			[Display(Name = "CreateDateTime", Order = 5)]
			public DateTime CreateDateTime { get; set; }

			[Required(ErrorMessage = "ChangeDateTime")]
			[Display(Name = "ChangeDateTime", Order = 6)]
			public DateTime ChangeDateTime { get; set; }

		}
	}
}
