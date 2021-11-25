using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace  DALEFModel
{
   
	[MetadataTypeAttribute(typeof(EntityMenu.EntityMenuMetadata))]
	public partial class EntityMenu
	{
		internal sealed class EntityMenuMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private EntityMenuMetadata()
			{
			}

			[Key]
			[Display(Name = "EntityMenu", Order = 1)]
			public int EntityMenuID { get; set; }

			[Display(Name = "EntityMenuParent", Order = 2)]
			public int? EntityMenuParentID { get; set; }

			[Display(Name = "Page", Order = 3)]
			public int? PageID { get; set; }

			[StringLength(50,ErrorMessage = "MenuDisplayName")]
			[Required(ErrorMessage = "MenuDisplayName")]
			[Display(Name = "MenuDisplayName", Order = 4)]
			public string MenuDisplayName { get; set; }

			[StringLength(50,ErrorMessage = "Url")]
			[Display(Name = "Url", Order = 5)]
			public string Url { get; set; }

			[StringLength(50,ErrorMessage = "Param1")]
			[Display(Name = "Param1", Order = 6)]
			public string Param1 { get; set; }

			[StringLength(50,ErrorMessage = "Param1Value")]
			[Display(Name = "Param1Value", Order = 7)]
			public string Param1Value { get; set; }

			[StringLength(50,ErrorMessage = "Param2")]
			[Display(Name = "Param2", Order = 8)]
			public string Param2 { get; set; }

			[StringLength(50,ErrorMessage = "Param2Value")]
			[Display(Name = "Param2Value", Order = 9)]
			public string Param2Value { get; set; }

			[Display(Name = "Sequence", Order = 10)]
			public int? Sequence { get; set; }

			[Display(Name = "ParentSequence", Order = 11)]
			public int? ParentSequence { get; set; }

			[Display(Name = "CreatedBy", Order = 12)]
			public int? CreatedByID { get; set; }
            

		}
	}
}
