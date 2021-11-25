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
    
    
    
    public partial class Biometric : BaseClass
    {
        public Biometric()
        {
            this.AttendanceBiometric = new HashSet<AttendanceBiometric>();
    		
            this.PersonBiometric = new HashSet<PersonBiometric>();
    		
            this.StudentBiometric = new HashSet<StudentBiometric>();
    		
        }
    
        // Primitive properties
    
    	
    	public int BiometricID { get; set; }
    	
    	public byte[] Code { get; set; }
    	
    	public int StpBiometricTypeID { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    	
    	public Nullable<int> ChangedByID { get; set; }
    
    		// Navigation properties
    
    	    
    	
    		public ICollection<AttendanceBiometric> AttendanceBiometric { get; set; }
    	
    		
    		
        
    		public Organization Organization { get; set; }
    	
    		
    		
        
    		public StpData StpData { get; set; }
    	
    		
    		
        
    		public User User { get; set; }
    	
    		
    		
        
    		public User User1 { get; set; }
    	
    	    
    	
    		public ICollection<PersonBiometric> PersonBiometric { get; set; }
    	
    	    
    	
    		public ICollection<StudentBiometric> StudentBiometric { get; set; }
    	
    	}
}