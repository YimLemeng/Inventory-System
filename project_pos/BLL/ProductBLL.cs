using project_pos.DAL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project_pos.BLL
{
    public class ProductBLL : IDiscountable
    {

        private readonly ProductDAL _productDAL = new ProductDAL();

        public DataTable GetAllProducts() => _productDAL.GetAllProduct().Tables[0];

        public decimal ApplyDiscount(decimal amount, decimal discountPercent)
        {
            return amount - (amount * discountPercent / 100);
        }

        public bool Save(Product pro)
        {
            if (pro.UnitPrice <= 0) throw new Exception("Invalid Price");
            if (pro.StockQty < 0) throw new Exception("Invalid Stock Quantity");
            return _productDAL.Save(pro);
        }
        public bool Update(Product pro)
        {
            if (pro.UnitPrice <= 0) throw new Exception("Invalid Price");
            if (pro.StockQty < 0) throw new Exception("Invalid Stock Quantity");
            return _productDAL.Update(pro);
        }
        public bool Delete(int id)
        {
            return _productDAL.Delete(id);
        }

        // Business rule: buy 10 units or more of the same product = 5% quantity discount
        public decimal CalculateLineTotal(Product product, int qty)
        {
            decimal subtotal = product.UnitPrice * qty;
            if (qty >= 10)
            {
                subtotal = ApplyDiscount(subtotal, 5);
            }
            return subtotal;
        }
        public void ReduceStock(int productId, int qty) => _productDAL.ReduceStock(productId, qty);
    }
}
