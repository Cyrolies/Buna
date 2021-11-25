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
	[MetadataTypeAttribute(typeof(EntityField.EntityFieldMetadata))]
	public partial class EntityField
	{
		internal sealed class EntityFieldMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private EntityFieldMetadata()
			{
			}

			[Display(Name = "Activity", Order = 1)]
			public int ActivityID { get; set; }

			[Display(Name = "Add box spinners", Order = 2)]
			public int AddBoxSpinners { get; set; }

			[Display(Name = "Calendar maximum value", Order = 3)]
			public DateTime CalendarMaxValue { get; set; }

			[Display(Name = "Calendar minimum value", Order = 4)]
			public DateTime CalendarMinValue { get; set; }

			[Display(Name = "Combobox default text", Order = 5)]
			public string ComboBoxDefaultText { get; set; }

			[Display(Name = "Combobox display field", Order = 6)]
			public int ComboBoxDisplayFieldID { get; set; }

			[Display(Name = "Created by", Order = 7)]
			public int CreatedByID { get; set; }

			[Display(Name = "Date created", Order = 8)]
			public DateTime CreateDateTime { get; set; }

			[Required(ErrorMessage = " This field is required = Display name")]
			[Display(Name = "Display name", Order = 9)]
			public string DisplayName { get; set; }

			[Required(ErrorMessage = " This field is required = Entity field data type")]
			[Display(Name = "Entity field data type", Order = 10)]
			public int EntityFieldDataTypeID { get; set; }

			[Key]
			[Display(Name = "EntityField", Order = 11)]
			public string EntityFieldID { get; set; }

			[Required(ErrorMessage = " This field is required = Entity field name")]
			[Display(Name = "Entity field name", Order = 12)]
			public string EntityFieldName { get; set; }

			[Display(Name = "Entity", Order = 13)]
			public int EntityID { get; set; }

			[Display(Name = "Groupbox name", Order = 14)]
			public string GroupBoxName { get; set; }

			[Display(Name = "Is Active", Order = 15)]
			public bool IsActive { get; set; }

			[Display(Name = "Is a custom field", Order = 16)]
			public bool IsCustomField { get; set; }

			[Display(Name = "Is a foreign key", Order = 17)]
			public bool IsForeignKey { get; set; }

			[Display(Name = "Is hidden", Order = 18)]
			public bool IsHidden { get; set; }

			[Display(Name = "Is in grid display", Order = 19)]
			public bool IsInGridDisplay { get; set; }

			[Display(Name = "Is a mandatory field", Order = 20)]
			public bool IsMandatory { get; set; }

			[Display(Name = "Is a primary key", Order = 21)]
			public bool IsPrimaryKey { get; set; }

			[Display(Name = "Show a tool tip", Order = 22)]
			public bool IsToolTip { get; set; }

			[Display(Name = "Max field length", Order = 23)]
			public string Max { get; set; }

			[Display(Name = "Min field length", Order = 24)]
			public string Min { get; set; }

			[Display(Name = "Order no ", Order = 25)]
			public string ControlOrderNo { get; set; }

			[Required(ErrorMessage = " This field is required = Control type")]
			[Display(Name = "Control type", Order = 26)]
			public int StcControlTypeID { get; set; }

			[Display(Name = "Status", Order = 27)]
			public int StcStatusID { get; set; }

			[Display(Name = "Tab name", Order = 28)]
			public string TabName { get; set; }

			[Display(Name = "Tab order no", Order = 29)]
			public string TabOrderNo { get; set; }

			[Display(Name = "Upload file save to path", Order = 30)]
			public string UploadFileSaveToPath { get; set; }

		}
	}
}
