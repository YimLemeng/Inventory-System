using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public class StockOut : BaseModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public string Reason { get; set; }
        public DateTime StockOutDate { get; set; } = DateTime.Now;
        public string Note { get; set; }

        public override string GetInfo() => $"StockOut #{Id} - {ProductName} x{Qty}";
    }
}
