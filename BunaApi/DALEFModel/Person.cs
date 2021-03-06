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
    
    
    
    public partial class Person : BaseClass
    {
        public Person()
        {
            this.Asset = new HashSet<Asset>();
    		
            this.Person1 = new HashSet<Person>();
    		
        }
    
        // Primitive properties
    
    	
    	public int PersonID { get; set; }
    	
    	public string Firstnames { get; set; }
    	
    	public string Surname { get; set; }
    	
    	public Nullable<int> StpTitleID { get; set; }
    	
    	public Nullable<int> StpPersonCategoryID { get; set; }
    	
    	public Nullable<int> StpPersonTypeID { get; set; }
    	
    	public Nullable<int> StpGenderID { get; set; }
    	
    	public Nullable<int> StpProvinceID { get; set; }
    	
    	public Nullable<int> StpDistrictID { get; set; }
    	
    	public Nullable<int> StpConstituencyID { get; set; }
    	
    	public Nullable<int> StpWardID { get; set; }
    	
    	public Nullable<int> StpCorrespondenceTypeID { get; set; }
    	
    	public string Email { get; set; }
    	
    	public Nullable<int> Age { get; set; }
    	
    	public string HomePhone { get; set; }
    	
    	public string MobilePhone { get; set; }
    	
    	public string WorkPhone { get; set; }
    	
    	public string PhysicalAddress { get; set; }
    	
    	public string PostalAddress { get; set; }
    	
    	public Nullable<int> ParentPersonID { get; set; }
    	
    	public Nullable<int> UserID { get; set; }
    	
    	public bool hasSmartDevice { get; set; }
    	
    	public bool hasFinance { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    	
    	public Nullable<int> ChangedByID { get; set; }
    
    		// Navigation properties
    
    	    
    	
    		public ICollection<Asset> Asset { get; set; }
    	
    		
    		
        
    		public Organization Organization { get; set; }
    	
    	    
    	
    		public ICollection<Person> Person1 { get; set; }
    	
    		
    		
        
    		public Person Person2 { get; set; }
    	
    		
    		
        
    		public StcData StcData { get; set; }
    	
    		
    		
        
    		public StpData StpData { get; set; }
    	
    		
    		
        
    		public StpData StpData1 { get; set; }
    	
    		
    		
        
    		public StpData StpData2 { get; set; }
    	
    		
    		
        
    		public StpData StpData3 { get; set; }
    	
    		
    		
        
    		public StpData StpData4 { get; set; }
    	
    		
    		
        
    		public StpData StpData5 { get; set; }
    	
    		
    		
        
    		public StpData StpData6 { get; set; }
    	
    		
    		
        
    		public StpData StpData7 { get; set; }
    	
    		
    		
        
    		public StpData StpData8 { get; set; }
    	
    		
    		
        
    		public User User { get; set; }
    	
    		
    		
        
    		public User User1 { get; set; }
    	
    		
    		
        
    		public User User2 { get; set; }
    	
    	}
}
