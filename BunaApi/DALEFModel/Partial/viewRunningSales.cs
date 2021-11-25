using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DALEFModel
{
    public partial class viewRunningSales
    {
        List<RunningSalesDetail> saledetail = new List<RunningSalesDetail>();

        public List<RunningSalesDetail> SaleDetail
        {
            get { return saledetail; }
            set { saledetail = value; }
        }
        string allDeliveryDetail = string.Empty;

        public string AllDeliveryDetail
        {
            get { return allDeliveryDetail; }
            set { allDeliveryDetail = value; }
        }
    }
}
