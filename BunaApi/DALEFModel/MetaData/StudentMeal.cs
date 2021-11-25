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
	[MetadataTypeAttribute(typeof(StudentMeal.StudentMealMetadata))]
	public partial class StudentMeal
	{
		internal sealed class StudentMealMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private StudentMealMetadata()
			{
			}

			[Display(Name = "StudentNo", Order = 1)]
			public int StudentNo { get; set; }

			[StringLength(150,MinimumLength = 0,ErrorMessage = " MealPreference Maximum length is = 150 Minimum length is = 0")]
			[Display(Name = "MealPreference", Order = 2)]
			public string MealPreference { get; set; }

			[Display(Name = "MealTime", Order = 3)]
			public TimeSpan MealTime { get; set; }

			[Display(Name = "CreatedBy", Order = 4)]
			public int CreatedByID { get; set; }

			[Display(Name = "ChangedBy", Order = 5)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 6)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 7)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "StudentMeal", Order = 8)]
			public int StudentMealID { get; set; }

			[Display(Name = "Student", Order = 9)]
			public int StudentID { get; set; }

			[StringLength(150,MinimumLength = 0,ErrorMessage = " Allergies Maximum length is = 150 Minimum length is = 0")]
			[Display(Name = "Allergies", Order = 10)]
			public string Allergies { get; set; }

		}
	}
}
