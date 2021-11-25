using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Common;

namespace DALEFModel
{

	/// <summary>
	/// Partial class 
	/// Author: Robin Cyrolies
	/// </summary>
	public partial class Consumable
	{

			public string OrgDesc { get; set; }
			public string StatusDesc { get; set; }
			public string CreatedByDesc { get; set; }
			public string ChangedByDesc { get; set; }
			public string UOMForOpenAndCloseDesc { get; set; }
			public string ConsumableTypeDesc { get; set; }
			public string UnitOfMeasureDesc { get; set; }
			public string AssetDesc { get; set; }
			public string UsedOnAssetDesc { get; set; }
	}
}
