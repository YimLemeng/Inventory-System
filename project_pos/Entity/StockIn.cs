using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public class StockIn : BaseModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime StockInDate { get; set; } = DateTime.Now;
        public string Note { get; set; }

        public override string GetInfo() => $"StockIn #{Id} - {ProductName} x{Qty}";
    }
}
