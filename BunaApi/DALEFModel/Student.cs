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
    
    
    
    public partial class Student : BaseClass
    {
        public Student()
        {
            this.Attendance = new HashSet<Attendance>();
    		
            this.StudentBiometric = new HashSet<StudentBiometric>();
    		
            this.StudentMeal = new HashSet<StudentMeal>();
    		
        }
    
        // Primitive properties
    
    	
    	public int StudentID { get; set; }
    	
    	public int StudentNo { get; set; }
    	
    	public string AdmissionNo { get; set; }
    	
    	public string Age { get; set; }
    	
    	public string AgeGroup { get; set; }
    	
    	public string Campus { get; set; }
    	
    	public string Cell { get; set; }
    	
    	public string Class { get; set; }
    	
    	public string Email { get; set; }
    	
    	public string House { get; set; }
    	
    	public string TutorGroup { get; set; }
    	
    	public string Firstname { get; set; }
    	
    	public string Surname { get; set; }
    	
    	public string Grade { get; set; }
    	
    	public string Gender { get; set; }
    	
    	public Nullable<int> StpMealTypeID { get; set; }
    	
    	public string Allergies { get; set; }
    	
    	public Nullable<int> CreatedByID { get; set; }
    	
    	public Nullable<int> ChangedByID { get; set; }
    
    		// Navigation properties
    
    	    
    	
    		public ICollection<Attendance> Attendance { get; set; }
    	
    		
    		
        
    		public Organization Organization { get; set; }
    	
    		
    		
        
    		public StpData StpData { get; set; }
    	
    		
    		
        
    		public User User { get; set; }
    	
    		
    		
        
    		public User User1 { get; set; }
    	
    	    
    	
    		public ICollection<StudentBiometric> StudentBiometric { get; set; }
    	
    	    
    	
    		public ICollection<StudentMeal> StudentMeal { get; set; }
    	
    	}
}
