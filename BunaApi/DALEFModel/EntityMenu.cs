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
    
    
    
    public partial class EntityMenu : BaseClass
    {
        // Primitive properties
    
    	
    	public int EntityMenuID { get; set; }
    	
    	public Nullable<int> EntityMenuParentID { get; set; }
    	
    	public Nullable<int> EntityID { get; set; }
    	
    	public Nullable<int> PageID { get; set; }
    	
    	public string MenuDisplayName { get; set; }
    	
    	public string Url { get; set; }
    	
    	public string Param1 { get; set; }
    	
    	public string Param1Value { get; set; }
    	
    	public string Param2 { get; set; }
    	
    	public string Param2Value { get; set; }
    	
    	public Nullable<int> Sequence { get; set; }
    	
    	public Nullable<int> ParentSequence { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    
    		// Navigation properties
    
    		
    		
        
    		public Page Page { get; set; }
    	
    		
    		
        
    		public StcData StcData { get; set; }
    	
    	}
}
