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
    
    
    
    public partial class StcData : BaseClass
    {
        public StcData()
        {
            this.Activity = new HashSet<Activity>();
    		
            this.Asset = new HashSet<Asset>();
    		
            this.Consumable = new HashSet<Consumable>();
    		
            this.Contact = new HashSet<Contact>();
    		
            this.EntityField = new HashSet<EntityField>();
    		
            this.EntityMenu = new HashSet<EntityMenu>();
    		
            this.EntityResource = new HashSet<EntityResource>();
    		
            this.Page = new HashSet<Page>();
    		
            this.Person = new HashSet<Person>();
    		
            this.StpData = new HashSet<StpData>();
    		
            this.User = new HashSet<User>();
    		
            this.UserRole = new HashSet<UserRole>();
    		
            this.UserRoleActivity = new HashSet<UserRoleActivity>();
    		
            this.UserRoleActivity1 = new HashSet<UserRoleActivity>();
    		
            this.UserRoleNotificationLevel = new HashSet<UserRoleNotificationLevel>();
    		
        }
    
        // Primitive properties
    
    	
    	public int StcDataID { get; set; }
    	
    	public int StcDataTypeID { get; set; }
    	
    	public string Abbreviation { get; set; }
    	
    	public string Description { get; set; }
    
    		// Navigation properties
    
    	    
    	
    		public ICollection<Activity> Activity { get; set; }
    	
    	    
    	
    		public ICollection<Asset> Asset { get; set; }
    	
    	    
    	
    		public ICollection<Consumable> Consumable { get; set; }
    	
    	    
    	
    		public ICollection<Contact> Contact { get; set; }
    	
    	    
    	
    		public ICollection<EntityField> EntityField { get; set; }
    	
    	    
    	
    		public ICollection<EntityMenu> EntityMenu { get; set; }
    	
    	    
    	
    		public ICollection<EntityResource> EntityResource { get; set; }
    	
    		
    		
        
    		public Organization Organization { get; set; }
    	
    	    
    	
    		public ICollection<Page> Page { get; set; }
    	
    	    
    	
    		public ICollection<Person> Person { get; set; }
    	
    		
    		
        
    		public StcDataType StcDataType { get; set; }
    	
    	    
    	
    		public ICollection<StpData> StpData { get; set; }
    	
    	    
    	
    		public ICollection<User> User { get; set; }
    	
    	    
    	
    		public ICollection<UserRole> UserRole { get; set; }
    	
    	    
    	
    		public ICollection<UserRoleActivity> UserRoleActivity { get; set; }
    	
    	    
    	
    		public ICollection<UserRoleActivity> UserRoleActivity1 { get; set; }
    	
    	    
    	
    		public ICollection<UserRoleNotificationLevel> UserRoleNotificationLevel { get; set; }
    	
    	}
}
