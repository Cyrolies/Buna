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
	[MetadataTypeAttribute(typeof(Entity.EntityMetadata))]
	public partial class Entity
	{
		internal sealed class EntityMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private EntityMetadata()
			{
			}

			[Display(Name = "Activity", Order = 1)]
			public int ActivityID { get; set; }

			[Display(Name = "CreatedBy", Order = 2)]
			public int CreatedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 3)]
			public string CreateDateTime { get; set; }

			[Key]
			[Display(Name = "Entity", Order = 4)]
			public int EntityID { get; set; }

			[Display(Name = "Is Active", Order = 5)]
			public bool IsActive { get; set; }

			[Display(Name = "IsMultiGrid", Order = 6)]
			public bool IsMultiGrid { get; set; }

			[Display(Name = "Field data is multi language", Order = 7)]
			public bool IsMultiLanguage { get; set; }

			[Display(Name = "Is a tabbed form", Order = 8)]
			public bool IsTabbedForm { get; set; }

			[Required(ErrorMessage = " This field is required = Max No of fields in groupbox")]
			[Display(Name = "Max No of fields in groupbox", Order = 9)]
			public string MaxNoFieldsInGrpBox { get; set; }

			[Required(ErrorMessage = " This field is required = Manager &Contoller Name")]
			[Display(Name = "Manager &Contoller Name", Order = 10)]
			public string MngCtlrName { get; set; }

			[Required(ErrorMessage = " This field is required = Name")]
			[Display(Name = "Name", Order = 11)]
			public string Name { get; set; }

			[Required(ErrorMessage = " This field is required = Path to edit template forms")]
			[Display(Name = "Path to edit template forms", Order = 12)]
			public string PathEditTemplate { get; set; }

			[Required(ErrorMessage = " This field is required = Path to setup forms")]
			[Display(Name = "Path to setup forms", Order = 13)]
			public string PathSetupForm { get; set; }

			[Display(Name = "Status", Order = 14)]
			public int StcStatusID { get; set; }

			[Required(ErrorMessage = " This field is required = TableName")]
			[Display(Name = "TableName", Order = 15)]
			public string TableName { get; set; }

			[Display(Name = "ChangeDateTime", Order = 16)]
			public string ChangeDateTime { get; set; }

			[Display(Name = "ActivityName", Order = 17)]
			public string ActivityName { get; set; }

		}
	}
}
