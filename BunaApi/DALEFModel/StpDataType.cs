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
    
    
    
    public partial class StpDataType : BaseClass
    {
        public StpDataType()
        {
            this.StpData = new HashSet<StpData>();
    		
        }
    
        // Primitive properties
    
    	
    	public int StpDataTypeID { get; set; }
    	
    	public string AppDataType { get; set; }
    	
    	public string AppDataSection { get; set; }
    	
    	public bool IsSystem { get; set; }
    
    		// Navigation properties
    
    		
    		
        
    		public Organization Organization { get; set; }
    	
    	    
    	
    		public ICollection<StpData> StpData { get; set; }
    	
    	}
}
