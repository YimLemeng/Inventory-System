using project_pos.DAL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.BLL
{
    public class StockOutBLL
    {
        private readonly StockOutDAL _stockOutDAL = new StockOutDAL();
        public bool Save(StockOut stockOut)
        {
            if (stockOut.Qty <= 0) throw new Exception("Invalid Quantity");
            if (string.IsNullOrWhiteSpace(stockOut.Reason)) throw new Exception("Reason is required for Stock Out");
            return _stockOutDAL.Save(stockOut);
        }
        public List<StockOut> GetAllStockOuts() => _stockOutDAL.GetAllStockOut();
    }
}
