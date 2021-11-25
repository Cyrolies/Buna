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
	[MetadataTypeAttribute(typeof(VisitListMobileUser.VisitListMobileUserMetadata))]
	public partial class VisitListMobileUser
	{
		internal sealed class VisitListMobileUserMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private VisitListMobileUserMetadata()
			{
			}

			[Display(Name = "VisitList", Order = 1)]
			public int VisitListID { get; set; }

			[Display(Name = "MobilelUser", Order = 2)]
			public int MobileUserID { get; set; }

			[Key]
			[Display(Name = "Id", Order = 3)]
			public int Id { get; set; }

		}
	}
}
