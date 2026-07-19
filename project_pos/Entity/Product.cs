using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public class Product : BaseModel
    {
        private int _productID;
        private string _productName;
        private string _barcode;
        private int? _categoryId; 
        private string _categoryName;
        private decimal _unitPrice;
        private int _stockQty;
        public int ProductID
        {
            get => _productID;
            set => _productID = value;
        }
        public string ProductName
        {
            get => _productName;
            set => _productName = value;
        }
        public string Barcode
        {
            get => _barcode;
            set => _barcode = value;
        }
        public int? CategoryId 
        {
            get => _categoryId;
            set => _categoryId = value;
        }
        public string CategoryName
        {
            get => _categoryName;
            set => _categoryName = value;
        }
        public decimal UnitPrice
        {
            get => _unitPrice;
            set => _unitPrice = value >= 0 ? value : 0;
        }
        public int StockQty
        {
            get => _stockQty;
            set => _stockQty = value;
        }
        public override string GetInfo() => $"{ProductName} ({CategoryName}) - {UnitPrice:N2}";
        public override string ToString() => ProductName;
    }
}
