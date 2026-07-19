using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.DAL
{
    public class StockInDAL : DBConnection
    {
        public bool Save(StockIn stockIn)
        {
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            try
            {
                string sql = @"INSERT INTO StockIn (ProductId, Qty, UnitCost, SupplierId, StockInDate, Note)
                               VALUES (@ProductId, @Qty, @UnitCost, @SupplierId, @StockInDate, @Note)";

                SqlCommand cmd = new SqlCommand(sql, con, transaction);
                cmd.Parameters.AddWithValue("@ProductId", stockIn.ProductId);
                cmd.Parameters.AddWithValue("@Qty", stockIn.Qty);
                cmd.Parameters.AddWithValue("@UnitCost", stockIn.UnitCost);
                cmd.Parameters.AddWithValue("@SupplierId", (object)stockIn.SupplierId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StockInDate", stockIn.StockInDate);
                cmd.Parameters.AddWithValue("@Note", (object)stockIn.Note ?? DBNull.Value);
                int rows = cmd.ExecuteNonQuery();

                // ✅ បន្ថែម Stock ចូល Products ភ្លាមៗ ក្នុង transaction តែមួយ
                string sqlUpdate = "UPDATE Products SET StockQty = StockQty + @Qty WHERE ProductId = @ProductId";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, con, transaction);
                cmdUpdate.Parameters.AddWithValue("@Qty", stockIn.Qty);
                cmdUpdate.Parameters.AddWithValue("@ProductId", stockIn.ProductId);
                cmdUpdate.ExecuteNonQuery();

                transaction.Commit();
                return rows > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error saving stock in: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public List<StockIn> GetAllStockIn()
        {
            var list = new List<StockIn>();
            string sql = @"SELECT s.StockInId, s.ProductId, p.ProductName, s.Qty, s.UnitCost, 
                          s.SupplierId, sup.SupplierName, s.StockInDate, s.Note
                           FROM StockIn s
                           JOIN Products p ON s.ProductId = p.ProductID
                           LEFT JOIN Suppliers sup ON s.SupplierId = sup.SupplierId
                           ORDER BY s.StockInDate DESC";
            var cmd = new SqlCommand(sql, con);
            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new StockIn
                {
                    Id = (int)reader["StockInId"],
                    ProductId = (int)reader["ProductId"],
                    ProductName = reader["ProductName"].ToString(),
                    Qty = (int)reader["Qty"],
                    UnitCost = Convert.ToDecimal(reader["UnitCost"]),
                    SupplierId = reader["SupplierId"] == DBNull.Value ? (int?)null : (int)reader["SupplierId"],
                    SupplierName = reader["SupplierName"] == DBNull.Value ? "" : reader["SupplierName"].ToString(),
                    StockInDate = (DateTime)reader["StockInDate"],
                    Note = reader["Note"] == DBNull.Value ? "" : reader["Note"].ToString()
                });
            }
            con.Close();
            return list;
        }
    }
}
