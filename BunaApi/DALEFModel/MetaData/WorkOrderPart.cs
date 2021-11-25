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
	[MetadataTypeAttribute(typeof(WorkOrderPart.WorkOrderPartMetadata))]
	public partial class WorkOrderPart
	{
		internal sealed class WorkOrderPartMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private WorkOrderPartMetadata()
			{
			}

			[Required(ErrorMessage = " This field is required = Quantity")]
			[Display(Name = "Quantity", Order = 1)]
			public int Quantity { get; set; }

			[Display(Name = "ReturnDate", Order = 2)]
			public DateTime ReturnDate { get; set; }

			[Display(Name = "CreatedBy", Order = 3)]
			public int CreatedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 4)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 5)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "WorkOrderPart", Order = 6)]
			public int WorkOrderPartID { get; set; }

			[Required(ErrorMessage = " This field is required = Part")]
			[Display(Name = "Part", Order = 7)]
			public int PartID { get; set; }

			[Required(ErrorMessage = " This field is required = WorkOrder")]
			[Display(Name = "WorkOrder", Order = 8)]
			public int WorkOrderID { get; set; }

		}
	}
}
