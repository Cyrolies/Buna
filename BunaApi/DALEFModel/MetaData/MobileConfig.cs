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
	[MetadataTypeAttribute(typeof(MobileConfig.MobileConfigMetadata))]
	public partial class MobileConfig
	{
		internal sealed class MobileConfigMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private MobileConfigMetadata()
			{
			}

			[Key]
			[Display(Name = "Id", Order = 1)]
			public int Id { get; set; }

			[Required(ErrorMessage = "EnableCheckerName")]
			[Display(Name = "EnableCheckerName", Order = 2)]
			public bool EnableCheckerName { get; set; }

			[Required(ErrorMessage = "EnableCheckerPin")]
			[Display(Name = "EnableCheckerPin", Order = 3)]
			public bool EnableCheckerPin { get; set; }

			[Required(ErrorMessage = "EnableCheckersSignature")]
			[Display(Name = "EnableCheckersSignature", Order = 4)]
			public bool EnableCheckersSignature { get; set; }

			[Required(ErrorMessage = "EnableMessaging")]
			[Display(Name = "EnableMessaging", Order = 5)]
			public bool EnableMessaging { get; set; }

			[Required(ErrorMessage = "EnableCheckList")]
			[Display(Name = "EnableCheckList", Order = 6)]
			public bool EnableCheckList { get; set; }

			[Required(ErrorMessage = "EnableCheckListLeadApproval")]
			[Display(Name = "EnableCheckListLeadApproval", Order = 7)]
			public bool EnableCheckListLeadApproval { get; set; }

			[Required(ErrorMessage = "EnableCustomerEdit")]
			[Display(Name = "EnableCustomerEdit", Order = 8)]
			public bool EnableCustomerEdit { get; set; }

			[Required(ErrorMessage = "EnableSalesOrderHistory")]
			[Display(Name = "EnableSalesOrderHistory", Order = 9)]
			public bool EnableSalesOrderHistory { get; set; }

			[Required(ErrorMessage = "EnableSignOffFreeText")]
			[Display(Name = "EnableSignOffFreeText", Order = 10)]
			public bool EnableSignOffFreeText { get; set; }

			[Required(ErrorMessage = "EnableHelpers")]
			[Display(Name = "EnableHelpers", Order = 11)]
			public bool EnableHelpers { get; set; }

			[Required(ErrorMessage = "EnableHelpersList")]
			[Display(Name = "EnableHelpersList", Order = 12)]
			public bool EnableHelpersList { get; set; }

			[Required(ErrorMessage = "EnablePrePrintOfDeliveryList")]
			[Display(Name = "EnablePrePrintOfDeliveryList", Order = 13)]
			public bool EnablePrePrintOfDeliveryList { get; set; }

			[Required(ErrorMessage = "EnableCheckInPlantList")]
			[Display(Name = "EnableCheckInPlantList", Order = 14)]
			public bool EnableCheckInPlantList { get; set; }

			[Required(ErrorMessage = "CreateDateTime")]
			[Display(Name = "CreateDateTime", Order = 15)]
			public DateTime CreateDateTime { get; set; }

			[Required(ErrorMessage = "ChangeDateTime")]
			[Display(Name = "ChangeDateTime", Order = 16)]
			public DateTime ChangeDateTime { get; set; }

		}
	}
}
