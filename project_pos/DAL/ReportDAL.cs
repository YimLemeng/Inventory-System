using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.DAL
{
    public class ReportDAL : DBConnection
    {
        public DataSet GetSalesReport(DateTime fromDate, DateTime toDate)
        {
            DataSet dt = new DataSet();
            string sql = @"SELECT r.ReceiptId, r.ReceiptDate, c.Name AS CustomerName, 
                                  r.SubTotal, r.DiscountPercent, r.GrandTotal, pm.MethodName
                           FROM Receipts r
                           LEFT JOIN Customers c ON r.CustomerId = c.CustomerId
                           LEFT JOIN PaymentMethods pm ON r.PaymentMethodId = pm.PaymentMethodId
                           WHERE r.ReceiptDate BETWEEN @FromDate AND @ToDate
                           ORDER BY r.ReceiptDate DESC";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
            cmd.Parameters.AddWithValue("@ToDate", toDate.Date.AddDays(1).AddSeconds(-1));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        public DataSet GetInventoryAlertReport()
        {
            string sql = @"SELECT p.ProductId, p.ProductName, p.Barcode, p.CategoryId, 
                          c.CategoryName, p.UnitPrice, p.StockQty 
                           FROM Products p
                           LEFT JOIN Category c ON p.CategoryId = c.CategoryId
                           WHERE p.IsActive = 1 AND p.StockQty <= 10
                           ORDER BY p.StockQty ASC";

            DataSet ds = new DataSet();
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, con)) { da.Fill(ds); }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching inventory alert report: " + ex.Message);
            }
        }
    }
}
