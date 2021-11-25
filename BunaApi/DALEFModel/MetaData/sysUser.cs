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
	[MetadataTypeAttribute(typeof(sysUser.sysUserMetadata))]
	public partial class sysUser
	{
		internal sealed class sysUserMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private sysUserMetadata()
			{
			}

			[Key]
			[Display(Name = "Id", Order = 1)]
			public long Id { get; set; }

			[StringLength(100,ErrorMessage = " UserName Max length = 100")]
			[Required(ErrorMessage = "UserName")]
			[Display(Name = "UserName", Order = 2)]
			public string UserName { get; set; }

			[StringLength(100,ErrorMessage = " Password Max length = 100")]
			[Required(ErrorMessage = "Password")]
			[Display(Name = "Password", Order = 3)]
			public string Password { get; set; }

			[StringLength(100,ErrorMessage = " Type Max length = 100")]
			[Required(ErrorMessage = "Type")]
			[Display(Name = "Type", Order = 4)]
			public string Type { get; set; }

			[Display(Name = "CreateDateTime", Order = 5)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "Status", Order = 6)]
			public string Status { get; set; }

			[Display(Name = "DeletionFlag", Order = 7)]
			public bool DeletionFlag { get; set; }

		}
	}
}
