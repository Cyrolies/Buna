using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace  DALEFModel
{

	[MetadataTypeAttribute(typeof(UserRoleUser.UserRoleUserMetadata))]
	public partial class UserRoleUser
	{
		internal sealed class UserRoleUserMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private UserRoleUserMetadata()
			{
			}

			[Required(ErrorMessage = "User")]
			[Display(Name = "User", Order = 1)]
			public int UserID { get; set; }

			[Required(ErrorMessage = "UserRole")]
			[Display(Name = "UserRole", Order = 2)]
			public int UserRoleID { get; set; }

            //private User _user = null;
            //public User User { 
            //    get
            //    {
            //       return _user;
            //    }
            //    set
            //    {
            //        _user = value;
            //    }
		}
	}
}
