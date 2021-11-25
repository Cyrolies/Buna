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
	[MetadataTypeAttribute(typeof(VisitList.VisitListMetadata))]
	public partial class VisitList
	{
		internal sealed class VisitListMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private VisitListMetadata()
			{
			}

			[Key]
			[Display(Name = "Id", Order = 1)]
			public string Id { get; set; }

			[Display(Name = "ChangeDateTime", Order = 2)]
			public DateTime ChangeDateTime { get; set; }

			[Display(Name = "CreateDateTime", Order = 3)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "Sat", Order = 4)]
			public bool Sat { get; set; }

			[Display(Name = "Sequence", Order = 5)]
			public int Sequence { get; set; }

			[Display(Name = "Sun", Order = 6)]
			public bool Sun { get; set; }

			[StringLength(1,MinimumLength = 0,ErrorMessage = " SyncStatus Maximum length is = 1 Minimum length is = 0")]
			[Display(Name = "SyncStatus", Order = 7)]
			public string SyncStatus { get; set; }

			[Display(Name = "Thu", Order = 8)]
			public bool Thu { get; set; }

			[Display(Name = "Tue", Order = 9)]
			public bool Tue { get; set; }

			[StringLength(18,MinimumLength = 0,ErrorMessage = " VehicleId Maximum length is = 18 Minimum length is = 0")]
			[Display(Name = "VehicleId", Order = 10)]
			public string VehicleId { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " VisitGroup Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "VisitGroup", Order = 11)]
			public string VisitGroup { get; set; }

			[Display(Name = "Fri", Order = 12)]
			public bool Fri { get; set; }

			[Display(Name = "Mon", Order = 13)]
			public bool Mon { get; set; }

			[StringLength(30,MinimumLength = 0,ErrorMessage = " Name Maximum length is = 30 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Name")]
			[Display(Name = "Name", Order = 14)]
			public string Name { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " DriverNumber Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "DriverNumber", Order = 15)]
			public string DriverNumber { get; set; }

			[StringLength(30,MinimumLength = 0,ErrorMessage = " VisitName Maximum length is = 30 Minimum length is = 0")]
			[Display(Name = "VisitName", Order = 16)]
			public string VisitName { get; set; }

			[StringLength(3,MinimumLength = 0,ErrorMessage = " VisitPlanType Maximum length is = 3 Minimum length is = 0")]
			[Display(Name = "VisitPlanType", Order = 17)]
			public string VisitPlanType { get; set; }

			[Display(Name = "Wed", Order = 18)]
			public bool Wed { get; set; }

			[StringLength(20,MinimumLength = 0,ErrorMessage = " ExecutionDate Maximum length is = 20 Minimum length is = 0")]
			[Display(Name = "ExecutionDate", Order = 19)]
			public string ExecutionDate { get; set; }

			[StringLength(6,MinimumLength = 0,ErrorMessage = " Route Maximum length is = 6 Minimum length is = 0")]
			[Display(Name = "Route", Order = 20)]
			public string Route { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " VisitListId Maximum length is = 10 Minimum length is = 0")]
			[Display(Name = "VisitListId", Order = 21)]
			public string VisitListId { get; set; }

			[StringLength(10,MinimumLength = 0,ErrorMessage = " CustomerId Maximum length is = 10 Minimum length is = 0")]
			[Key]
			[Display(Name = "CustomerId", Order = 22)]
			public string CustomerId { get; set; }

		}
	}
}
