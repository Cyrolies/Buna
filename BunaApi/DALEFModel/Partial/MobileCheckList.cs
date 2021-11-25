using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DALEFModel
{
    public partial class MobileCheckList
    {
        public List<MobileVehicleItem> MobileVehicleItemList { get; set; }
    }

    public class MobileVehicleItem
    {
        public string VehicleType { get; set; }
        public string VehicleTypeCode { get; set; }
        public bool IsSelected { get; set; }
    }
}
