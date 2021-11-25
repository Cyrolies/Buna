using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace  DALEFModel
{

	[MetadataTypeAttribute(typeof(EntityResource.EntityResourceMetadata))]
	public partial class EntityResource
	{
		internal sealed class EntityResourceMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private EntityResourceMetadata()
			{
			}

			[Key]
			[Display(Name = "Resource", Order = 1)]
			public int ResourceID { get; set; }

			[StringLength(50,ErrorMessage = "ResourceKey")]
			[Required(ErrorMessage = "ResourceKey")]
			[Display(Name = "ResourceKey", Order = 2)]
			public string ResourceKey { get; set; }

			[StringLength(500,ErrorMessage = "ResourceValue")]
			[Required(ErrorMessage = "ResourceValue")]
			[Display(Name = "ResourceValue", Order = 3)]
			public string ResourceValue { get; set; }

			[StringLength(50,ErrorMessage = "ResourceCulture")]
			[Required(ErrorMessage = "ResourceCulture")]
			[Display(Name = "ResourceCulture", Order = 4)]
			public string ResourceCulture { get; set; }

		}
	}
}
