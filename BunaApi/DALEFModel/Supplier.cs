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
    
    
    
    public partial class Supplier : BaseClass
    {
        public Supplier()
        {
            this.Product = new HashSet<Product>();
    		
        }
    
        // Primitive properties
    
    	
    	public int SupplierID { get; set; }
    	
    	public string Name { get; set; }
    	
    	public string ContactName { get; set; }
    	
    	public string Countries { get; set; }
    	
    	public string Description { get; set; }
    	
    	public string Email { get; set; }
    	
    	public string MobilePhone { get; set; }
    	
    	public string WorkPhone { get; set; }
    	
    	public string PhysicalAddress { get; set; }
    	
    	public string PostalAddress { get; set; }
    	
    	public string ImageName { get; set; }
    	
    	public byte[] FileData { get; set; }
    	
    	public string ContentType { get; set; }
    	
    	public Nullable<int> UserID { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    	
    	public Nullable<int> ChangedByID { get; set; }
    
    		// Navigation properties
    
    		
    		
        
    		public Organization Organization { get; set; }
    	
    	    
    	
    		public ICollection<Product> Product { get; set; }
    	
    		
    		
        
    		public User User { get; set; }
    	
    		
    		
        
    		public User User1 { get; set; }
    	
    		
    		
        
    		public User User2 { get; set; }
    	
    	}
}
