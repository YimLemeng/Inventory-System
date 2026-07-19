using project_pos.DAL;
using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.BLL
{
    public class SupplierBLL
    {
        private readonly SupplierDAL _supplierDAL = new SupplierDAL();
        public bool Save(Supplier sup)
        {
            if (string.IsNullOrWhiteSpace(sup.SupplierName)) throw new Exception("Supplier Name is required");
            return _supplierDAL.Save(sup);
        }
        public bool Update(Supplier sup)
        {
            if (sup.Id <= 0) throw new Exception("Invalid Supplier ID");
            if (string.IsNullOrWhiteSpace(sup.SupplierName)) throw new Exception("Supplier Name is required");
            return _supplierDAL.Update(sup);
        }
        public bool Delete(int id) => _supplierDAL.Delete(id);
        public DataSet GetAllSuppliers() => _supplierDAL.GetAllSupplier();
    }
}
