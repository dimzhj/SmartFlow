using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.views
{
    public class TransMaster
    {
        public string TransportNo { get; set; }
        public string SupCltName { get; set; }
        public string TransDate { get; set; }
        public string WarehouseName { get; set; }
        public string UserName { get; set; }
        public List<TransDetail> TransDetails
        {
            get
            {
                return m_transDetails;
            }
        }
        public List<TransDetail> m_transDetails = new List<TransDetail>();

        public decimal TotalPrice
        {
            get
            {
                return m_transDetails.Sum(o => o.Price * o.Quantity);
            }
        }
    }

    public class TransDetail
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
