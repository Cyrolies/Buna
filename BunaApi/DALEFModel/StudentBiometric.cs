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
    
    
    
    public partial class StudentBiometric : BaseClass
    {
        // Primitive properties
    
    	
    	public int StudentBiometricID { get; set; }
    	
    	public int StudentID { get; set; }
    	
    	public int BiometricID { get; set; }
    
    		// Navigation properties
    
    		
    		
        
    		public Biometric Biometric { get; set; }
    	
    		
    		
        
    		public Student Student { get; set; }
    	
    	}
}