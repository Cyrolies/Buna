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
	[MetadataTypeAttribute(typeof(UserRoleActivity.UserRoleActivityMetadata))]
	public partial class UserRoleActivity
	{
		internal sealed class UserRoleActivityMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private UserRoleActivityMetadata()
			{
			}

			[Key]
			[Display(Name = "UserRoleActivity", Order = 1)]
			public int UserRoleActivityID { get; set; }

			[Required(ErrorMessage = "UserRole")]
			[Display(Name = "UserRole", Order = 2)]
			public int UserRoleID { get; set; }

			[Required(ErrorMessage = "Activity")]
			[Display(Name = "Activity", Order = 3)]
			public int ActivityID { get; set; }

			[Required(ErrorMessage = "Permission")]
			[Display(Name = "Permission", Order = 4)]
			public int StcPermissionID { get; set; }

			[Display(Name = "CreatedBy", Order = 5)]
			public int? CreatedByID { get; set; }

		}
	}
}
