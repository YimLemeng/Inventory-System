using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public class ReceiptDetail : BaseModel
    {
        private int _receiptId;
        private int _productId;
        private string _productName;
        private decimal _price;
        private int _qty;
        private decimal _lineTotal;

        public int ReceiptId
        {
            get => _receiptId;
            set => _receiptId = value;
        }

        public int ProductId
        {
            get => _productId;
            set => _productId = value;
        }

        public string ProductName
        {
            get => _productName;
            set => _productName = value;
        }

        public decimal Price
        {
            get => _price;
            set => _price = value;
        }

        public int Qty
        {
            get => _qty;
            set => _qty = value;
        }

        public decimal LineTotal
        {
            get => _lineTotal;
            set => _lineTotal = value;
        }

        public override string GetInfo() => $"{ProductName} x{Qty} = {LineTotal:N2}";
    }
}
