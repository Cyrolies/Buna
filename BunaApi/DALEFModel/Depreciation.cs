//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DALEFModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    
    
    public partial class Depreciation : BaseClass
    {
        // Primitive properties
    
    	
    	public int DepreciationID { get; set; }
    	
    	public Nullable<int> AssetID { get; set; }
    	
    	public Nullable<int> PartID { get; set; }
    	
    	public int Year { get; set; }
    	
    	public decimal Value { get; set; }
    	
    	public int StpValueType { get; set; }
    	
    	public decimal CurrentValue { get; set; }
    	
    	public Nullable<decimal> SalvageValue { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    	
    	public Nullable<int> ChangedByID { get; set; }
    
    }
}
