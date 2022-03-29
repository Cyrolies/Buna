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
	[MetadataTypeAttribute(typeof(Production.ProductionMetadata))]
	public partial class Production
	{
		internal sealed class ProductionMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private ProductionMetadata()
			{
			}

			[StringLength(150,MinimumLength = 0,ErrorMessage = " Farm Maximum length is = 150 Minimum length is = 0")]
			[Display(Name = "Farm", Order = 1)]
			public string Farm { get; set; }

			[Display(Name = "ProductionType", Order = 2)]
			[Required(ErrorMessage = "Production type is required")]
			public int StpProductionTypeID { get; set; }

			[Display(Name = "FishSpecies", Order = 3)]
			[Required(ErrorMessage = "Fish species is required")]
			public int StpFishSpeciesID { get; set; }

			[Display(Name = "UnitOfMeasure", Order = 4)]
			
			public int StpUnitOfMeasureID { get; set; }

			[Display(Name = "Quantity", Order = 5)]
			[Required(ErrorMessage = "Quantity is required")]
			public int Quantity { get; set; }

			[Display(Name = "CreatedBy", Order = 6)]
			public int CreatedByID { get; set; }

			[Display(Name = "ChangedBy", Order = 7)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 8)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 9)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "Production", Order = 10)]
			public int ProductionID { get; set; }

		}
	}
}
