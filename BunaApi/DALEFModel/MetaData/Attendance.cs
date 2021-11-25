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
	[MetadataTypeAttribute(typeof(Attendance.AttendanceMetadata))]
	public partial class Attendance
	{
		internal sealed class AttendanceMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private AttendanceMetadata()
			{
			}

			[StringLength(250,MinimumLength = 0,ErrorMessage = " Location Maximum length is = 250 Minimum length is = 0")]
			[Display(Name = "Location", Order = 1)]
			public string Location { get; set; }

			[Display(Name = "CreatedBy", Order = 2)]
			public int CreatedByID { get; set; }

			[Display(Name = "ChangedBy", Order = 3)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 4)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 5)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "Attendance", Order = 6)]
			public int AttendanceID { get; set; }

			[Display(Name = "Person", Order = 7)]
			public int PersonID { get; set; }

		}
	}
}
