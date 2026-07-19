using project_pos.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.BLL
{
    public class ReportBLL
    {
        private readonly ReportDAL _dal = new ReportDAL();
        public DataTable GetSalesReport(DateTime from, DateTime to)
        {
            if (from > to) throw new Exception("From Date must be before To Date");
            return _dal.GetSalesReport(from, to).Tables[0];
        }
        public DataTable GetInventoryAlertReport() => _dal.GetInventoryAlertReport().Tables[0];
    }

}
