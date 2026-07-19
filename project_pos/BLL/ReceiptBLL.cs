using project_pos.DAL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.BLL
{
    public class ReceiptBLL : IDiscountable
    {
        private readonly ReceiptDAL _receiptDAL = new ReceiptDAL();
        private readonly ProductBLL _productBLL = new ProductBLL();

        public decimal ApplyDiscount(decimal amount, decimal discountPercent)
        {
            return amount - (amount * discountPercent / 100);
        }

        public decimal CalculateSubTotal(List<ReceiptDetail> details) => details.Sum(d => d.LineTotal);

        public decimal CalculateGrandTotal(List<ReceiptDetail> details, decimal discountPercent)
        {
            decimal subTotal = CalculateSubTotal(details);
            return ApplyDiscount(subTotal, discountPercent);
        }

        public int SaveReceipt(Receipt receipt, List<ReceiptDetail> details)
        {
            int receiptId = _receiptDAL.SaveReceipt(receipt, details);

            foreach (var d in details)
            {
                _productBLL.ReduceStock(d.ProductId, d.Qty);
            }

            return receiptId;
        }
        public List<Receipt> GetAllReceipts() => _receiptDAL.GetAllReceipts();
    }
}
