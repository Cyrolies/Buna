using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DALEFModel
{
    public partial class VisitListMobileUser
    {
        public MobileUser MobileUser { get; set; }
        public string MobileUserDesc { get; set; }
        public string VisitListDesc { get; set; }
        public string FullName { get; set; }
        public string MainWarehouseCode { get; set; }
        public string MainWarehouseDesc { get; set; }
        public string VanWarehouseCode { get; set; }

        public string VanWarehouseDesc { get; set; }

        public string UserBy { get; set; }
    }

    
}
