using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.DAL
{
    public class StockOutDAL : DBConnection
    {
        public bool Save(StockOut stockOut)
        {
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            try
            {
                string sql = @"INSERT INTO StockOut (ProductId, Qty, Reason, StockOutDate, Note)
                               VALUES (@ProductId, @Qty, @Reason, @StockOutDate, @Note)";

                SqlCommand cmd = new SqlCommand(sql, con, transaction);
                cmd.Parameters.AddWithValue("@ProductId", stockOut.ProductId);
                cmd.Parameters.AddWithValue("@Qty", stockOut.Qty);
                cmd.Parameters.AddWithValue("@Reason", (object)stockOut.Reason ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StockOutDate", stockOut.StockOutDate);
                cmd.Parameters.AddWithValue("@Note", (object)stockOut.Note ?? DBNull.Value);
                int rows = cmd.ExecuteNonQuery();

                // ✅ កាត់ Stock ចេញពី Products ភ្លាមៗ ក្នុង transaction តែមួយ
                string sqlUpdate = "UPDATE Products SET StockQty = StockQty - @Qty WHERE ProductId = @ProductId";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, con, transaction);
                cmdUpdate.Parameters.AddWithValue("@Qty", stockOut.Qty);
                cmdUpdate.Parameters.AddWithValue("@ProductID", stockOut.ProductId);
                cmdUpdate.ExecuteNonQuery();
                transaction.Commit();
                return rows > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error saving stock out: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public List<StockOut> GetAllStockOut()
        {
            var list = new List<StockOut>();
            string sql = @"SELECT s.StockOutId, s.ProductId, p.ProductName, s.Qty, s.Reason, s.StockOutDate, s.Note
                           FROM StockOut s
                           JOIN Products p ON s.ProductId = p.ProductID
                           ORDER BY s.StockOutDate DESC";

            var cmd = new SqlCommand(sql, con);
            con.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new StockOut
                {
                    Id = (int)reader["StockOutId"],
                    ProductId = (int)reader["ProductId"],
                    ProductName = reader["ProductName"].ToString(),
                    Qty = (int)reader["Qty"],
                    Reason = reader["Reason"] == DBNull.Value ? "" : reader["Reason"].ToString(),
                    StockOutDate = (DateTime)reader["StockOutDate"],
                    Note = reader["Note"] == DBNull.Value ? "" : reader["Note"].ToString()
                });
            }
            con.Close();
            return list;
        }
    }
}
