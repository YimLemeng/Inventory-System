using project_pos.DAL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.BLL
{
    public class PurchaseBLL
    {
        private readonly PurchaseDAL _purchaseDAL = new PurchaseDAL();
        public int SavePurchase(Purchase purchase, List<PurchaseDetail> details)
        {
            if (purchase.SupplierId <= 0)
                throw new Exception("Invalid Supplier selected.");
            if (details == null || !details.Any())
                throw new Exception("Purchase cart cannot be empty.");
            foreach (var item in details)
            {
                if (item.Qty <= 0) throw new Exception($"Invalid Quantity for product ID {item.ProductId}.");
                if (item.UnitCost < 0) throw new Exception($"Invalid Unit Cost for product ID {item.ProductId}.");
            }
            purchase.TotalAmount = details.Sum(d => d.LineTotal);
            return _purchaseDAL.SavePurchase(purchase, details);
        }
        public List<Purchase> GetAllPurchases() => _purchaseDAL.GetAllPurchases();
        public List<PurchaseDetail> GetPurchaseDetails(int purchaseId) => _purchaseDAL.GetPurchaseDetails(purchaseId);
    }
}

