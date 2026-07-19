using project_pos.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.BLL
{
    public class PaymentMethodBLL
    {
        private readonly PaymentMethodDAL _paymentMethodDAL = new PaymentMethodDAL();
        public DataTable GetAllPaymentMethod() => _paymentMethodDAL.GetAllPaymentMethod().Tables[0];
    }
}
