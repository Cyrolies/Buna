using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DALEFModel
{
    public class EntityVM
    {
         public EntityVM()
        {
            this.EntityFields = new HashSet<EntityFieldVM>();
        }
    
        // Primitive properties
    
    	
    	public int EntityID { get; set; }
    	
    	public Nullable<int> ActivityID { get; set; }
    	
    	public string Name { get; set; }
    	
    	public string TableName { get; set; }
    	
    	public bool IsMultiLanguage { get; set; }
    	
    	public bool IsTabbedFrom { get; set; }
    
    		// Navigation properties


        public IEnumerable<EntityFieldVM> EntityFields { get; set; }
    	
    		
    	//	private Activity Activity { get; set; }
    	
    	
    }

}
