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
	[MetadataTypeAttribute(typeof(PersonBiometric.PersonBiometricMetadata))]
	public partial class PersonBiometric
	{
		internal sealed class PersonBiometricMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private PersonBiometricMetadata()
			{
			}

			[Key]
			[Display(Name = "PersonBiometric", Order = 1)]
			public int PersonBiometricID { get; set; }

			[Display(Name = "Person", Order = 2)]
			public int PersonID { get; set; }

			[Display(Name = "Biometric", Order = 3)]
			public int BiometricID { get; set; }

		}
	}
}
