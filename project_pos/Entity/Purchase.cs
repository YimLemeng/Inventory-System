using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public class Purchase : BaseModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; } = true;
        public List<PurchaseDetail> Details { get; set; } = new List<PurchaseDetail>();
        public override string GetInfo() => $"Purchase #{Id} - Total: {TotalAmount:N2}";
    }
}
