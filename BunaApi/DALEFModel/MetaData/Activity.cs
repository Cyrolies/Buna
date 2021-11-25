using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace  DALEFModel
{

    /// <summary>
    /// Metadata class which has all the dataannotation attributes 
    /// </summary>
   
	[MetadataTypeAttribute(typeof(Activity.ActivityMetadata))]
	public partial class Activity
	{
		internal sealed class ActivityMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private ActivityMetadata()
			{
			}

			[Key]
			[Display(Name = "Activity", Order = 1)]
			public int ActivityID { get; set; }

			[Required(ErrorMessage = "Activity Group is Required")]
			[Display(Name = "Activity group", Order = 2)]
			public int StpActivityGroupID { get; set; }

			[StringLength(20,ErrorMessage = "Activity name maximun length is 20")]
			[Required(ErrorMessage = "Activity name is mandatory")]
			[Display(Name = "Activity name", Order = 3)]
			public string ActivityName { get; set; }

			[Display(Name = "CreatedBy", Order = 4)]
			public int? CreatedByID { get; set; }

		}
	}
}
