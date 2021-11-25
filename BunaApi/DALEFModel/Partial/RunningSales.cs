using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DALEFModel
{
   
        public class RunningSales : BaseClass
        {
            // Primitive properties
            
            public Nullable<System.DateTime> DeliveryDate { get; set; }

            public string Status { get; set; }

            public string DeviceSerialNumber { get; set; }

            public Nullable<System.DateTime> CreateDateTime { get; set; }

            public Nullable<System.DateTime> ShipmentDate { get; set; }

            public string ShippingPoint { get; set; }

            public string DriverName { get; set; }

            public string DriverNumber { get; set; }

            public Nullable<int> TotalDelivery { get; set; }

            public Nullable<int> TotalCompletedDelivery { get; set; }

            public Nullable<int> TotalCancelled { get; set; }

            public Nullable<int> TotalBcp { get; set; }

            public Nullable<int> TotalRemaining { get; set; }

            public Nullable<decimal> TotalProductsOrdered { get; set; }

            public Nullable<decimal> TotalProductsDelivered { get; set; }

            public string ShipmentNumber { get; set; }

            private List<RunningSalesDetail> runningSaleDetail = new List<RunningSalesDetail>();
                

            public List<RunningSalesDetail> RunningSaleDetail
            {
              get { return runningSaleDetail; }
              set { runningSaleDetail = value; }
            }
        }
   

}
