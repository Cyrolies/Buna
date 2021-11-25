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
    
    
    
    public partial class Organization : BaseClass
    {
        public Organization()
        {
            this.Activity = new HashSet<Activity>();
    		
            this.Application = new HashSet<Application>();
    		
            this.Asset = new HashSet<Asset>();
    		
            this.Attendance = new HashSet<Attendance>();
    		
            this.Audit = new HashSet<Audit>();
    		
            this.Biometric = new HashSet<Biometric>();
    		
            this.Consumable = new HashSet<Consumable>();
    		
            this.Contact = new HashSet<Contact>();
    		
            this.Entity = new HashSet<Entity>();
    		
            this.EntityResource = new HashSet<EntityResource>();
    		
            this.HitecJob = new HashSet<HitecJob>();
    		
            this.Part = new HashSet<Part>();
    		
            this.Person = new HashSet<Person>();
    		
            this.StcData = new HashSet<StcData>();
    		
            this.StcDataType = new HashSet<StcDataType>();
    		
            this.StockTake = new HashSet<StockTake>();
    		
            this.StpData = new HashSet<StpData>();
    		
            this.StpDataType = new HashSet<StpDataType>();
    		
            this.Student = new HashSet<Student>();
    		
            this.StudentMeal = new HashSet<StudentMeal>();
    		
            this.User1 = new HashSet<User>();
    		
            this.WorkOrder = new HashSet<WorkOrder>();
    		
            this.WorkOrderPart = new HashSet<WorkOrderPart>();
    		
        }
    
        // Primitive properties
    
    	
    	public string OrganizationName { get; set; }
    	
    	public string Description { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    
    		// Navigation properties
    
    	    
    	
    		public ICollection<Activity> Activity { get; set; }
    	
    	    
    	
    		public ICollection<Application> Application { get; set; }
    	
    	    
    	
    		public ICollection<Asset> Asset { get; set; }
    	
    	    
    	
    		public ICollection<Attendance> Attendance { get; set; }
    	
    	    
    	
    		public ICollection<Audit> Audit { get; set; }
    	
    	    
    	
    		public ICollection<Biometric> Biometric { get; set; }
    	
    	    
    	
    		public ICollection<Consumable> Consumable { get; set; }
    	
    	    
    	
    		public ICollection<Contact> Contact { get; set; }
    	
    	    
    	
    		public ICollection<Entity> Entity { get; set; }
    	
    	    
    	
    		public ICollection<EntityResource> EntityResource { get; set; }
    	
    	    
    	
    		public ICollection<HitecJob> HitecJob { get; set; }
    	
    		
    		
        
    		public User User { get; set; }
    	
    	    
    	
    		public ICollection<Part> Part { get; set; }
    	
    	    
    	
    		public ICollection<Person> Person { get; set; }
    	
    	    
    	
    		public ICollection<StcData> StcData { get; set; }
    	
    	    
    	
    		public ICollection<StcDataType> StcDataType { get; set; }
    	
    	    
    	
    		public ICollection<StockTake> StockTake { get; set; }
    	
    	    
    	
    		public ICollection<StpData> StpData { get; set; }
    	
    	    
    	
    		public ICollection<StpDataType> StpDataType { get; set; }
    	
    	    
    	
    		public ICollection<Student> Student { get; set; }
    	
    	    
    	
    		public ICollection<StudentMeal> StudentMeal { get; set; }
    	
    	    
    	
    		public ICollection<User> User1 { get; set; }
    	
    	    
    	
    		public ICollection<WorkOrder> WorkOrder { get; set; }
    	
    	    
    	
    		public ICollection<WorkOrderPart> WorkOrderPart { get; set; }
    	
    	}
}
