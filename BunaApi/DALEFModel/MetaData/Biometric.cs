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
	[MetadataTypeAttribute(typeof(Biometric.BiometricMetadata))]
	public partial class Biometric
	{
		internal sealed class BiometricMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private BiometricMetadata()
			{
			}

			[Display(Name = "Code", Order = 1)]
			public byte[] Code { get; set; }

			[Display(Name = "BiometricType", Order = 2)]
			public int StpBiometricTypeID { get; set; }

			[Display(Name = "CreatedBy", Order = 3)]
			public int CreatedByID { get; set; }

			[Display(Name = "ChangedBy", Order = 4)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 5)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 6)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "Biometric", Order = 7)]
			public int BiometricID { get; set; }

		}
	}
}
