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
	[MetadataTypeAttribute(typeof(Product.ProductMetadata))]
	public partial class Product
	{
		internal sealed class ProductMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private ProductMetadata()
			{
			}

			[Display(Name = "CreatedBy", Order = 1)]
			public int CreatedByID { get; set; }

			[Display(Name = "ChangedBy", Order = 2)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 3)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 4)]
			public DateTime ChangeDateTime { get; set; }

			[StringLength(50,MinimumLength = 0,ErrorMessage = " Name Maximum length is = 50 Minimum length is = 0")]
			[Display(Name = "Name", Order = 5)]
			public string Name { get; set; }

			[StringLength(350,MinimumLength = 0,ErrorMessage = " Description Maximum length is = 350 Minimum length is = 0")]
			[Display(Name = "Description", Order = 6)]
			public string Description { get; set; }

			[Display(Name = "Price", Order = 7)]
			public Decimal Price { get; set; }

			[Display(Name = "Currency", Order = 8)]
			public int StpCurrencyID { get; set; }

			[Display(Name = "Supplier", Order = 10)]
			public int SupplierID { get; set; }

			[Key]
			[Display(Name = "Product", Order = 11)]
			public int ProductID { get; set; }

		}
	}
}
