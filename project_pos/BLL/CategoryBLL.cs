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
    public class CategoryBLL
    {
        private readonly CategoryDAL _categoryDAL = new CategoryDAL();
        public bool Save(Category cat)
        {
            if (string.IsNullOrWhiteSpace(cat.CategoryName)) throw new Exception("Category Name is required");
            return _categoryDAL.Save(cat);
        }
        public bool Update(Category cat)
        {
            if (cat.Id <= 0) throw new Exception("Invalid Category ID");
            if (string.IsNullOrWhiteSpace(cat.CategoryName)) throw new Exception("Category Name is required");
            return _categoryDAL.Update(cat);
        }
        public bool Delete(int id) => _categoryDAL.Delete(id);
        public DataTable GetAllCategories() => _categoryDAL.GetAllCategory().Tables[0];
    }
}
