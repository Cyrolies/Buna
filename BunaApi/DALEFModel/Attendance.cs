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
    
    
    
    public partial class Attendance : BaseClass
    {
        public Attendance()
        {
            this.AttendanceBiometric = new HashSet<AttendanceBiometric>();
    		
        }
    
        // Primitive properties
    
    	
    	public int AttendanceID { get; set; }
    	
    	public Nullable<int> PersonID { get; set; }
    	
    	public Nullable<int> StudentID { get; set; }
    	
    	public string Location { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    	
    	public Nullable<int> ChangedByID { get; set; }
    
    		// Navigation properties
    
    		
    		
        
    		public Organization Organization { get; set; }
    	
    		
    		
        
    		public Person Person { get; set; }
    	
    		
    		
        
    		public Student Student { get; set; }
    	
    		
    		
        
    		public User User { get; set; }
    	
    		
    		
        
    		public User User1 { get; set; }
    	
    	    
    	
    		public ICollection<AttendanceBiometric> AttendanceBiometric { get; set; }
    	
    	}
}