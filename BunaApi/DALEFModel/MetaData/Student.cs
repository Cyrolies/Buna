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
	[MetadataTypeAttribute(typeof(Student.StudentMetadata))]
	public partial class Student
	{
		internal sealed class StudentMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private StudentMetadata()
			{
			}

			[Display(Name = "StudentNo", Order = 1)]
			public int StudentNo { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " AdmissionNo Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "AdmissionNo", Order = 2)]
			public string AdmissionNo { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Age Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Age", Order = 3)]
			public string Age { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " AgeGroup Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "AgeGroup", Order = 4)]
			public string AgeGroup { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Campus Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Campus", Order = 5)]
			public string Campus { get; set; }

			[StringLength(100,MinimumLength = 0,ErrorMessage = " Cell Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "Cell", Order = 6)]
			public string Cell { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Class Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Class", Order = 7)]
			public string Class { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Email Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Email", Order = 8)]
			public string Email { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " House Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "House", Order = 9)]
			public string House { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " TutorGroup Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "TutorGroup", Order = 10)]
			public string TutorGroup { get; set; }

			[StringLength(150,MinimumLength = 0,ErrorMessage = " Firstname Maximum length is = 150 Minimum length is = 0")]
			[Display(Name = "Firstname", Order = 11)]
			public string Firstname { get; set; }

			[StringLength(150,MinimumLength = 0,ErrorMessage = " Surname Maximum length is = 150 Minimum length is = 0")]
			[Display(Name = "Surname", Order = 12)]
			public string Surname { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Grade Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Grade", Order = 13)]
			public string Grade { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " Gender Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "Gender", Order = 14)]
			public string Gender { get; set; }

			[Display(Name = "Meal Preference", Order = 15)]
			public int StpMealTypeID { get; set; }

			[StringLength(150,MinimumLength = 0,ErrorMessage = " Allergies Maximum length is = 150 Minimum length is = 0")]
			[Display(Name = "Allergies", Order = 16)]
			public string Allergies { get; set; }

			[Display(Name = "CreatedBy", Order = 17)]
			public int CreatedByID { get; set; }

			[Display(Name = "ChangedBy", Order = 18)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 19)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 20)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "Student", Order = 21)]
			public int StudentID { get; set; }

		}
	}
}
