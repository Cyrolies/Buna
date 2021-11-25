using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Linq;
using System.Text;

namespace DALEFModel
{
    [DataContract(Namespace = "http://www.dataflo.co.za/DSD/SubmitCheckList")]
    public class SubmitCheckList
    {
        [DataMember(Name = "InCheckList", Order = 1)]
        public List<InCheckList> InCheckList { get; set; }
      
    }
}
