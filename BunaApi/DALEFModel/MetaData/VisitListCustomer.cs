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
	[MetadataTypeAttribute(typeof(VisitListCustomer.VisitListCustomerMetadata))]
	public partial class VisitListCustomer
	{
		internal sealed class VisitListCustomerMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private VisitListCustomerMetadata()
			{
			}

			[Display(Name = "VisitList", Order = 1)]
			public int VisitListID { get; set; }

			[Display(Name = "Customer", Order = 2)]
			public int CustomerID { get; set; }

			[Key]
			[Display(Name = "Id", Order = 3)]
			public int Id { get; set; }

		}
	}
}
