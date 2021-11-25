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
	[MetadataTypeAttribute(typeof(WorkOrder.WorkOrderMetadata))]
	public partial class WorkOrder
	{
		internal sealed class WorkOrderMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private WorkOrderMetadata()
			{
			}

			[Required(ErrorMessage = " This field is required = Client")]
			[Display(Name = "Client", Order = 1)]
			public int StpClientID { get; set; }

			[StringLength(150,MinimumLength = 0,ErrorMessage = " Client Contact Maximum length is = 150 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Client Contact")]
			[Display(Name = "Client Contact", Order = 2)]
			public string ClientContact { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Client Phone Maximum length is = 50 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Client Phone")]
			[Display(Name = "Client Phone", Order = 3)]
			public string ClientPhone { get; set; }

			[StringLength(500,MinimumLength = 0,ErrorMessage = " Address Maximum length is = 500 Minimum length is = 0")]
			[Display(Name = "Address", Order = 4)]
			public string Address { get; set; }

			[Display(Name = "Scheduled Date", Order = 5)]
			public DateTime ScheduledDate { get; set; }

			[Display(Name = "Start Time", Order = 6)]
			public TimeSpan StartTime { get; set; }

			[Display(Name = "End Time", Order = 7)]
			public TimeSpan EndTime { get; set; }

			[Display(Name = "Completed Date", Order = 8)]
			public DateTime CompletedDate { get; set; }

			[Display(Name = "Worked Hours", Order = 9)]
			public string WorkedHours { get; set; }

			[Display(Name = "Travel Time", Order = 10)]
			public TimeSpan TravelTime { get; set; }

			[Display(Name = "Is Quote", Order = 11)]
			public bool IsQuote { get; set; }

			[Display(Name = "Quote Accepted Date", Order = 12)]
			public DateTime QuoteAcceptedDate { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Quote Accepted By Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Quote Accepted By", Order = 13)]
			public string QuoteAcceptedBy { get; set; }

			[Display(Name = "Is Active", Order = 14)]
			public bool IsActive { get; set; }

			[Required(ErrorMessage = " This field is required = WorkOrderCode")]
			[Display(Name = "WorkOrderCode", Order = 15)]
			public string WorkOrderCode { get; set; }

			[StringLength(500,MinimumLength = 0,ErrorMessage = " Description Maximum length is = 500 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Description")]
			[Display(Name = "Description", Order = 16)]
			public string Description { get; set; }

			[Required(ErrorMessage = " This field is required = Work Order Type")]
			[Display(Name = "Work Order Type", Order = 17)]
			public int StpWorkOrderTypeID { get; set; }

			[Display(Name = "Created By  ", Order = 18)]
			public int CreatedByID { get; set; }

			[Display(Name = "Created Date", Order = 19)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "Changed Date", Order = 20)]
			public DateTime ChangeDateTime { get; set; }

			[Required(ErrorMessage = " This field is required = Technician")]
			[Display(Name = "Technician", Order = 21)]
			public int TechnicianID { get; set; }

			[Key]
			[Display(Name = "WorkOrder", Order = 22)]
			public int WorkOrderID { get; set; }

		}
	}
}
