using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DALEFModel
{
    
        public class RunningSalesDetail : BaseClass
        {
            // Primitive properties
            public Nullable<long> Id { get; set; }

            public string SoldTo { get; set; }

            public string SoldToName { get; set; }

            public string ShipmentNumber { get; set; }

            public Nullable<int> TotalProductsRemaining { get; set; }

            public Nullable<int> TotalZLRReturned { get; set; }

            public string TotalProductsRemainingDetail { get; set; }

            public Nullable<int> TotalCancelled { get; set; }

            public Nullable<int> TotalBcp { get; set; }

            public string CancelReason { get; set; }

            public Nullable<System.DateTime> CancelDate { get; set; }

            public Nullable<decimal> TotalProductDelivered { get; set; }

            public Nullable<decimal> TotalProductReturned { get; set; }

            public Nullable<decimal> TotalProductUpSold { get; set; }

            public string Type { get; set; }

            public string DeliveryDate { get; set; }

            public string Status { get; set; }

            public string ShippingPoint { get; set; }

            public string DriverNumber { get; set; }

            public string DriverName { get; set; }

        }

}
