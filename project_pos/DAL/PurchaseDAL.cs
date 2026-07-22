using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.DAL
{
    public class PurchaseDAL : DBConnection
    {
        public int SavePurchase(Purchase purchase, List<PurchaseDetail> details)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                string sqlPurchase = @"INSERT INTO Purchases (SupplierId, PurchaseDate, TotalAmount, Note) 
                               VALUES (@SupplierId, @PurchaseDate, @TotalAmount, @Note); 
                               SELECT SCOPE_IDENTITY();";

                SqlCommand cmdPurchase = new SqlCommand(sqlPurchase, con, transaction);
                cmdPurchase.Parameters.AddWithValue("@SupplierId", purchase.SupplierId);
                cmdPurchase.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate == default ? DateTime.Now : purchase.PurchaseDate);
                cmdPurchase.Parameters.AddWithValue("@TotalAmount", purchase.TotalAmount);
                cmdPurchase.Parameters.AddWithValue("@Note", string.IsNullOrWhiteSpace(purchase.Note) ? (object)DBNull.Value : purchase.Note);
                int purchaseId = Convert.ToInt32(cmdPurchase.ExecuteScalar());

                string sqlDetail = @"INSERT INTO PurchaseDetails (PurchaseId, ProductId, Qty, UnitCost, LineTotal) 
                             VALUES (@PurchaseId, @ProductId, @Qty, @UnitCost, @LineTotal)";

                string sqlStockIn = @"INSERT INTO StockIn (ProductId, Qty, UnitCost, SupplierId, StockInDate, Note) 
                              VALUES (@ProductId, @Qty, @UnitCost, @SupplierId, @StockInDate, @Note)";

                string sqlUpdateStock = @"UPDATE Products SET StockQty = StockQty + @Qty WHERE ProductID = @ProductId";

                SqlCommand cmdDetail = new SqlCommand(sqlDetail, con, transaction);
                SqlCommand cmdStockIn = new SqlCommand(sqlStockIn, con, transaction);
                SqlCommand cmdUpdateStock = new SqlCommand(sqlUpdateStock, con, transaction);

                foreach (var detail in details)
                {
                    cmdDetail.Parameters.Clear();
                    cmdDetail.Parameters.AddWithValue("@PurchaseId", purchaseId);
                    cmdDetail.Parameters.AddWithValue("@ProductId", detail.ProductId);
                    cmdDetail.Parameters.AddWithValue("@Qty", detail.Qty);
                    cmdDetail.Parameters.AddWithValue("@UnitCost", detail.UnitCost);
                    cmdDetail.Parameters.AddWithValue("@LineTotal", detail.LineTotal);
                    cmdDetail.ExecuteNonQuery();

                    cmdStockIn.Parameters.Clear();
                    cmdStockIn.Parameters.AddWithValue("@ProductId", detail.ProductId);
                    cmdStockIn.Parameters.AddWithValue("@Qty", detail.Qty);
                    cmdStockIn.Parameters.AddWithValue("@UnitCost", detail.UnitCost);
                    cmdStockIn.Parameters.AddWithValue("@SupplierId", detail.SupplierId > 0 ? (object)detail.SupplierId : DBNull.Value);
                    cmdStockIn.Parameters.AddWithValue("@StockInDate", purchase.PurchaseDate);
                    cmdStockIn.Parameters.AddWithValue("@Note", string.IsNullOrWhiteSpace(purchase.Note) ? (object)DBNull.Value : purchase.Note);
                    cmdStockIn.ExecuteNonQuery();

                    cmdUpdateStock.Parameters.Clear();
                    cmdUpdateStock.Parameters.AddWithValue("@Qty", detail.Qty);
                    cmdUpdateStock.Parameters.AddWithValue("@ProductId", detail.ProductId);
                    cmdUpdateStock.ExecuteNonQuery();
                }

                transaction.Commit();
                return purchaseId;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error saving purchase: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public List<Purchase> GetAllPurchases()
        {
            var list = new List<Purchase>();
            string sql = @"SELECT p.PurchaseId, p.SupplierId, s.SupplierName, p.PurchaseDate, p.TotalAmount, p.Note 
                           FROM Purchases p
                           JOIN Suppliers s ON p.SupplierId = s.SupplierId
                           WHERE p.IsActive = 1
                           ORDER BY p.PurchaseDate DESC";
            SqlCommand cmd = new SqlCommand(sql, con);
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Purchase
                        {
                            Id = Convert.ToInt32(reader["PurchaseId"]),
                            SupplierId = Convert.ToInt32(reader["SupplierId"]),
                            SupplierName = reader["SupplierName"].ToString(),
                            PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"]),
                            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                            Note = reader["Note"] == DBNull.Value ? "" : reader["Note"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving purchases: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return list;
        }
        public List<PurchaseDetail> GetPurchaseDetails(int purchaseId)
        {
            var list = new List<PurchaseDetail>();
            string sql = @"SELECT pd.PurchaseDetailId, pd.PurchaseId, pd.ProductId, pr.ProductName, pd.Qty, pd.UnitCost, pd.LineTotal
                           FROM PurchaseDetails pd
                           JOIN Products pr ON pd.ProductId = pr.ProductID
                           WHERE pd.PurchaseId = @PurchaseId";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);
            try
            {
                if (con.State == ConnectionState.Closed) con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PurchaseDetail
                        {
                            PurchaseDetailId = Convert.ToInt32(reader["PurchaseDetailId"]),
                            PurchaseId = Convert.ToInt32(reader["PurchaseId"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            Qty = Convert.ToInt32(reader["Qty"]),
                            UnitCost = Convert.ToDecimal(reader["UnitCost"]),
                            LineTotal = Convert.ToDecimal(reader["LineTotal"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving purchase details: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return list;
        }
    }
}

