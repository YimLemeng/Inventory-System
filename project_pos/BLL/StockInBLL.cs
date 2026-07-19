using project_pos.DAL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.BLL
{
    public class StockInBLL
    {
        private readonly StockInDAL _stockInDAL = new StockInDAL();

        public bool Save(StockIn stockIn)
        {
            if (stockIn.Qty <= 0) throw new Exception("Invalid Quantity");
            if (stockIn.UnitCost <= 0) throw new Exception("Invalid Unit Cost");
            return _stockInDAL.Save(stockIn);
        }

        public List<StockIn> GetAllStockIns() => _stockInDAL.GetAllStockIn();
    }
}
