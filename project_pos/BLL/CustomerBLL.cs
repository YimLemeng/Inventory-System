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
    public class CustomerBLL
    {
        private readonly CustomerDAL _customerDAL = new CustomerDAL();
        public DataTable GetAllCustomers() => _customerDAL.GatAllCustomer().Tables[0];
    }
}
