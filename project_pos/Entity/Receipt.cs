using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public class Receipt : BaseModel
    {
        private int _customerId;
        private DateTime _receiptDate = DateTime.Now;
        private decimal _subTotal;
        private decimal _discountPercent;
        private decimal _grandTotal;
        private int _paymentMethodId;

        public int CustomerId
        {
            get => _customerId;
            set => _customerId = value;
        }

        public DateTime ReceiptDate
        {
            get => _receiptDate;
            set => _receiptDate = value;
        }

        public decimal SubTotal
        {
            get => _subTotal;
            set => _subTotal = value;
        }

        public decimal DiscountPercent
        {
            get => _discountPercent;
            set => _discountPercent = value;
        }

        public decimal GrandTotal
        {
            get => _grandTotal;
            set => _grandTotal = value;
        }

        public int PaymentMethodId
        {
            get => _paymentMethodId;
            set => _paymentMethodId = value;
        }
        public override string GetInfo() => $"Receipt #{Id} - {GrandTotal:N2}";
    }
}
