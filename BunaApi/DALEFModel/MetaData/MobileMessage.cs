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
	[MetadataTypeAttribute(typeof(MobileMessage.MobileMessageMetadata))]
	public partial class MobileMessage
	{
		internal sealed class MobileMessageMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private MobileMessageMetadata()
			{
			}

			[Key]
			[Display(Name = "Id", Order = 1)]
			public int Id { get; set; }

			[StringLength(140,ErrorMessage = " MessageDetail Max length = 140")]
			[Required(ErrorMessage = "MessageDetail")]
			[Display(Name = "MessageDetail", Order = 2)]
			public string MessageDetail { get; set; }

			[Required(ErrorMessage = "CreateDateTime")]
			[Display(Name = "CreateDateTime", Order = 3)]
			public DateTime CreateDateTime { get; set; }

			[Required(ErrorMessage = "ChangeDateTime")]
			[Display(Name = "ChangeDateTime", Order = 4)]
			public DateTime ChangeDateTime { get; set; }

		}
	}
}
