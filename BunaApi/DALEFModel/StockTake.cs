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
    
    
    
    public partial class StockTake : BaseClass
    {
        // Primitive properties
    
    	
    	public int StockTakeID { get; set; }
    	
    	public int PartID { get; set; }
    	
    	public int Quantity { get; set; }
    	
    	public string Location { get; set; }
    	
    	public System.DateTime StockTakeDate { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    
    		// Navigation properties
    
    		
    		
        
    		public Organization Organization { get; set; }
    	
    		
    		
        
    		public Part Part { get; set; }
    	
    		
    		
        
    		public User User { get; set; }
    	
    	}
}
