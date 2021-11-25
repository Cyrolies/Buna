using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace  DALEFModel
{

	[MetadataTypeAttribute(typeof(UserRole.UserRoleMetadata))]
	public partial class UserRole
	{
		internal sealed class UserRoleMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private UserRoleMetadata()
			{
			}

			[Key]
			[Display(Name = "UserRole", Order = 1)]
			public int UserRoleID { get; set; }

			[StringLength(50,ErrorMessage = "RoleName")]
			[Required(ErrorMessage = "RoleName")]
			[Display(Name = "RoleName", Order = 2)]
			public string RoleName { get; set; }

			[Display(Name = "CreatedBy", Order = 3)]
			public int? CreatedByID { get; set; }

		}
	}
}
