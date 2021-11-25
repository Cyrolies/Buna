using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DALEFModel
{
   
    public partial class sapInImage
    {
        [DataMember(Name = "ShipmentId")]
        public string ShipmentId { get; set; }
    }
}
