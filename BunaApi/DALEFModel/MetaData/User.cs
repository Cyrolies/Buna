using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace  DALEFModel
{

	[MetadataTypeAttribute(typeof(User.UserMetadata))]
	public partial class User
	{
		internal sealed class UserMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private UserMetadata()
			{
			}

			[Key]
			[Display(Name = "User", Order = 1)]
			public int UserID { get; set; }

			[Display(Name = "Contact", Order = 2)]
			public int? ContactID { get; set; }

			[StringLength(50,ErrorMessage = "UserName")]
			[Required(ErrorMessage = "UserName")]
			[Display(Name = "UserName", Order = 3)]
			public string UserName { get; set; }

			[Display(Name = "UserPWD", Order = 4)]
			public string UserPWD { get; set; }

	    	[StringLength(150,ErrorMessage = "Email")]
			[Display(Name = "Email", Order = 6)]
			public string Email { get; set; }

			[Display(Name = "Registered", Order = 7)]
			public bool Registered { get; set; }

			[Display(Name = "Supervisor", Order = 8)]
			public int? SupervisorID { get; set; }

			[Display(Name = "Language", Order = 9)]
            //[Required(ErrorMessage = "Language")]
			public int? StpLanguageID { get; set; }

			[Display(Name = "Theme", Order = 10)]
            //[Required(ErrorMessage = "Theme")]
			public int? StpThemeID { get; set; }

			[Display(Name = "CreatedBy", Order = 11)]
			public int? CreatedByID { get; set; }

		}
	}
}
