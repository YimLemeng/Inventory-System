using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public class PurchaseDetail
    {
        public int PurchaseDetailId { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public int SupplierId { get; set; }
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal { get; set; }
    }
}
