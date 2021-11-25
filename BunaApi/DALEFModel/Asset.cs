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
    
    
    
    public partial class Asset : BaseClass
    {
        public Asset()
        {
            this.Asset1 = new HashSet<Asset>();
    		
            this.Consumable = new HashSet<Consumable>();
    		
            this.Consumable1 = new HashSet<Consumable>();
    		
        }
    
        // Primitive properties
    
    	
    	public int AssetID { get; set; }
    	
    	public string Code { get; set; }
    	
    	public string Name { get; set; }
    	
    	public string Description { get; set; }
    	
    	public Nullable<decimal> PurchasePrice { get; set; }
    	
    	public Nullable<System.DateTime> PurchaseDate { get; set; }
    	
    	public Nullable<decimal> PurchaseTax { get; set; }
    	
    	public Nullable<decimal> PurchaseCosts { get; set; }
    	
    	public int StpAssetCategoryID { get; set; }
    	
    	public Nullable<int> ParentAssetID { get; set; }
    	
    	public Nullable<int> AssignedToID { get; set; }
    	
    	public Nullable<System.DateTime> DateAssigned { get; set; }
    	
    	public byte[] Barcode { get; set; }
    	
    	public Nullable<decimal> SellPrice { get; set; }
    	
    	public bool IsSold { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    	
    	public Nullable<int> ChangedByID { get; set; }
    
    		// Navigation properties
    
    	    
    	
    		public ICollection<Asset> Asset1 { get; set; }
    	
    		
    		
        
    		public Asset Asset2 { get; set; }
    	
    		
    		
        
    		public Organization Organization { get; set; }
    	
    		
    		
        
    		public Person Person { get; set; }
    	
    		
    		
        
    		public StcData StcData { get; set; }
    	
    		
    		
        
    		public StpData StpData { get; set; }
    	
    		
    		
        
    		public User User { get; set; }
    	
    		
    		
        
    		public User User1 { get; set; }
    	
    	    
    	
    		public ICollection<Consumable> Consumable { get; set; }
    	
    	    
    	
    		public ICollection<Consumable> Consumable1 { get; set; }
    	
    	}
}
