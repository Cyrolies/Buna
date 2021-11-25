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
	[MetadataTypeAttribute(typeof(HitecJob.HitecJobMetadata))]
	public partial class HitecJob
	{
		internal sealed class HitecJobMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private HitecJobMetadata()
			{
			}

			[Key]
			[Display(Name = "HitecJob", Order = 1)]
			public int HitecJobID { get; set; }

			[StringLength(200,MinimumLength = 0,ErrorMessage = " Fullname Maximum length is = 200 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Fullname")]
			[Display(Name = "Fullname", Order = 2)]
			public string Fullname { get; set; }

			[StringLength(250,MinimumLength = 0,ErrorMessage = " Email Maximum length is = 250 Minimum length is = 0")]
			[Display(Name = "Email", Order = 3)]
			public string Email { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = "  Contact No Maximum length is = 50 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required =  Contact No")]
			[Display(Name = " Contact No", Order = 5)]
			public string ContactNumber { get; set; }

			[StringLength(1000,MinimumLength = 0,ErrorMessage = " Physical Address Guard Post Maximum length is = 1000 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Physical Address Guard Post")]
			[Display(Name = "Physical Address Guard Post", Order = 6)]
			public string PhysicalAddressGuardPost { get; set; }

			[Display(Name = "Number Of Guards Daytime", Order = 7)]
			public int NumberOfGuardsDaytime { get; set; }

			[Display(Name = "Number Of Guards Nighttime", Order = 8)]
			public int NumberOfGuardsNighttime { get; set; }

			[Required(ErrorMessage = " This field is required = StartDate")]
			[Display(Name = "Start Date", Order = 9)]
			public DateTime StartDate { get; set; }

			[Display(Name = "Start Time", Order = 10)]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
			public TimeSpan StartTime { get; set; }

			[Display(Name = "End Date", Order = 11)]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
			public DateTime EndDate { get; set; }

			[Display(Name = "End Time", Order = 12)]
			public TimeSpan EndTime { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Order Number Maximum length is = 50 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Order Number")]
			[Display(Name = "Order Number", Order = 13)]
			public string OrderNumber { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Total Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Total", Order = 14)]
			public string Total { get; set; }

			[Display(Name = "Invoiced Date", Order = 15)]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
			public DateTime InvoicedDate { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Invoice No Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Invoice No", Order = 16)]
			public string InvoiceNo { get; set; }

			[Display(Name = "Is Invoiced", Order = 17)]
			public bool IsInvoiced { get; set; }

			[Display(Name = "Is Complete", Order = 18)]
			public bool IsComplete { get; set; }

			[Display(Name = "Is Job Card Send", Order = 19)]
			public bool IsJobCardSend { get; set; }

			[StringLength(1000,MinimumLength = 0,ErrorMessage = " Billing Address  Maximum length is = 1000 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Billing Address ")]
			[Display(Name = "Billing Address ", Order = 20)]
			public string BillingAddress { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Client Maximum length is = 50 Minimum length is = 0")]
			[Required(ErrorMessage = " This field is required = Client")]
			[Display(Name = "Client", Order = 21)]
			public string Client { get; set; }

			[Display(Name = "Created By", Order = 22)]
			public int CreatedByID { get; set; }

			[Display(Name = "Create Date", Order = 23)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "Change Date", Order = 24)]
			public DateTime ChangeDateTime { get; set; }

		}
	}
}
