using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DALEFModel
{
    public class EntityFieldVM
    {
        public EntityFieldVM()
        {
          //  this.EntityFieldValues = new HashSet<EntityFieldValue>();
        }
    
        // Primitive properties

        
    	public int EntityFieldID { get; set; }
    	
    	public int EntityID { get; set; }
    	
    	public Nullable<int> StcEntityFieldDataTypeID { get; set; }
    	
    	public Nullable<int> StpDataTypeID { get; set; }
    	
    	public Nullable<int> ActivityID { get; set; }
        
    	public string EntityFieldName { get; set; }
    	
    	public string DisplayName { get; set; }
    	
    	public string GroupBoxName { get; set; }
    	
    	public string TabName { get; set; }
    	
    	public bool IsCustomField { get; set; }
    	
    	public bool IsInGridDisplay { get; set; }
    	
    	public bool IsMandatory { get; set; }
    	
    	public int MaxLength { get; set; }
    	
    	public bool IsActive { get; set; }
    	
    	public Nullable<int> MuniID { get; set; }
    	
    	public Nullable<int> StcStatusID { get; set; }
    	
    	public byte[] VersionNo { get; set; }
    
    		
    }
}
