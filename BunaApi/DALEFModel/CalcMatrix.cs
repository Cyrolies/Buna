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
    
    
    
    public partial class CalcMatrix : BaseClass
    {
        // Primitive properties
    
    	
    	public int CalcMatrixID { get; set; }
    	
    	public int Age { get; set; }
    	
    	public int Week { get; set; }
    	
    	public decimal Length { get; set; }
    	
    	public decimal Weight { get; set; }
    	
    	public Nullable<decimal> FeedBM { get; set; }
    	
    	public decimal FeedDay { get; set; }
    	
    	public decimal WeightGain { get; set; }
    	
    	public decimal Feed { get; set; }
    	
    	public decimal SFCR { get; set; }
    	
    	public Nullable<int> StpFishSpeciesID { get; set; }
    
    }
}