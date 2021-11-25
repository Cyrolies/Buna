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
	[MetadataTypeAttribute(typeof(StudentBiometric.StudentBiometricMetadata))]
	public partial class StudentBiometric
	{
		internal sealed class StudentBiometricMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private StudentBiometricMetadata()
			{
			}

			[Key]
			[Display(Name = "StudentBiometric", Order = 1)]
			public int StudentBiometricID { get; set; }

			[Key]
			[Display(Name = "Student", Order = 2)]
			public int StudentID { get; set; }

			[Key]
			[Display(Name = "Biometric", Order = 3)]
			public int BiometricID { get; set; }

		}
	}
}
