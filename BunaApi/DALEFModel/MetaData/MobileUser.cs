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
	[MetadataTypeAttribute(typeof(MobileUser.MobileUserMetadata))]
	public partial class MobileUser
	{
		internal sealed class MobileUserMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private MobileUserMetadata()
			{
			}

			[Key]
			[Display(Name = "Id", Order = 1)]
			public long Id { get; set; }

            //[StringLength(100,ErrorMessage = " Name Max length = 100")]
            //[Required(ErrorMessage = "Please enter a name")]
			[Display(Name = "Name", Order = 2)]
			public string Name { get; set; }

            //[StringLength(6,ErrorMessage = " Password Max length = 6")]
            //[Range(0, 999999, ErrorMessage = "Please enter a numeric value")]
            //[Required(ErrorMessage = "Please enter a password")]
			[Display(Name = "Password", Order = 3)]
			public string Password { get; set; }

			//[Required(ErrorMessage = "Please select a user type")]
			[Display(Name = "UserTypeId", Order = 4)]
			public int StpUserTypeId { get; set; }

			//[Required(ErrorMessage = "CreateDateTime")]
			[Display(Name = "CreateDateTime", Order = 5)]
			public DateTime CreateDateTime { get; set; }

			//[Required(ErrorMessage = "ChangeDateTime")]
			[Display(Name = "ChangeDateTime", Order = 6)]
			public DateTime ChangeDateTime { get; set; }

		}
	}
}
