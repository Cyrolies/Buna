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
	[MetadataTypeAttribute(typeof(Document.DocumentMetadata))]
	public partial class Document
	{
		internal sealed class DocumentMetadata
		{
			// Metadata classes are not meant to be instantiated.
			private DocumentMetadata()
			{
			}

			[StringLength(100,MinimumLength = 0,ErrorMessage = " DocumentName Maximum length is = 100 Minimum length is = 0")]
			[Display(Name = "DocumentName", Order = 1)]
			public string DocumentName { get; set; }

			[Display(Name = "FileData", Order = 2)]
			public byte[] FileData { get; set; }

			[StringLength(200,MinimumLength = 0,ErrorMessage = " ContentType Maximum length is = 200 Minimum length is = 0")]
			[Display(Name = "ContentType", Order = 3)]
			public string ContentType { get; set; }

			[StringLength(500,MinimumLength = 0,ErrorMessage = " Description Maximum length is = 500 Minimum length is = 0")]
			[Display(Name = "Description", Order = 4)]
			public string Description { get; set; }

			[Display(Name = "DocumentGroup", Order = 5)]
			public int StpDocumentGroupID { get; set; }

			[Display(Name = "CreatedBy", Order = 6)]
			public int CreatedByID { get; set; }

			[Display(Name = "ChangedBy", Order = 7)]
			public int ChangedByID { get; set; }

			[Display(Name = "CreateDateTime", Order = 8)]
			public DateTime CreateDateTime { get; set; }

			[Display(Name = "ChangeDateTime", Order = 9)]
			public DateTime ChangeDateTime { get; set; }

			[Key]
			[Display(Name = "Document", Order = 10)]
			public int DocumentID { get; set; }

		}
	}
}
