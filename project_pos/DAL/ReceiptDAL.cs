using project_pos.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.DAL
{
    public class ReceiptDAL : DBConnection
    {
        public int SaveReceipt(Receipt receipt, List<ReceiptDetail> details)
        {
            con.Open();
            SqlTransaction transaction = con.BeginTransaction();
            try
            {
                string sql = @"INSERT INTO Receipts (CustomerId, ReceiptDate, SubTotal, DiscountPercent, GrandTotal, PaymentMethodId) 
                               VALUES (@CustomerId, @ReceiptDate, @SubTotal, @DiscountPercent, @GrandTotal, @PaymentMethodId); 
                               SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(sql, con, transaction);
                cmd.Parameters.AddWithValue("@CustomerId", receipt.CustomerId);             // Terary Operateor
                cmd.Parameters.AddWithValue("@ReceiptDate", receipt.ReceiptDate == default ? DateTime.Now : receipt.ReceiptDate);
                cmd.Parameters.AddWithValue("@SubTotal", receipt.SubTotal);
                cmd.Parameters.AddWithValue("@DiscountPercent", receipt.DiscountPercent);
                cmd.Parameters.AddWithValue("@GrandTotal", receipt.GrandTotal);
                cmd.Parameters.AddWithValue("@PaymentMethodId", receipt.PaymentMethodId);

                // ប្រើ Convert.ToInt32() ដើម្បីជៀសវាងការខុសប្រភេទ Type របស់ DB
                int newReceiptId = Convert.ToInt32(cmd.ExecuteScalar());
                receipt.Id = newReceiptId;

                string sqlDetail = "INSERT INTO ReceiptDetails (ReceiptId, ProductId, Qty, Price, LineTotal) VALUES (@ReceiptId, @ProductId, @Qty, @Price, @LineTotal)";
                SqlCommand cmdDetail = new SqlCommand(sqlDetail, con, transaction);

                foreach (var detail in details)
                {
                    cmdDetail.Parameters.Clear();
                    cmdDetail.Parameters.AddWithValue("@ReceiptId", receipt.Id);
                    cmdDetail.Parameters.AddWithValue("@ProductId", detail.ProductId);
                    cmdDetail.Parameters.AddWithValue("@Qty", detail.Qty);
                    cmdDetail.Parameters.AddWithValue("@Price", detail.Price);
                    cmdDetail.Parameters.AddWithValue("@LineTotal", detail.LineTotal);
                    cmdDetail.ExecuteNonQuery();
                }

                transaction.Commit();
                return receipt.Id; 
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error saving receipt: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public List<Receipt> GetAllReceipts()
        {
            List<Receipt> receipts = new List<Receipt>();
            try
            {
                con.Open();
                string sql = "SELECT * FROM Receipts";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Receipt receipt = new Receipt
                    {
                        Id = Convert.ToInt32(reader["ReceiptId"]),
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        ReceiptDate = Convert.ToDateTime(reader["ReceiptDate"]),
                        SubTotal = Convert.ToDecimal(reader["SubTotal"]),
                        DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]),
                        GrandTotal = Convert.ToDecimal(reader["GrandTotal"]),
                        PaymentMethodId = Convert.ToInt32(reader["PaymentMethodId"])
                    };
                    receipts.Add(receipt);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving receipts: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return receipts;
        }

        //public List<ReceiptDetail> GetReceiptDetailsByReceiptId(int receiptId)
        //{
        //    List<ReceiptDetail> details = new List<ReceiptDetail>();
        //    try
        //    {
        //        con.Open();
        //        // ប្រើ JOIN ដើម្បីទាញយកឈ្មោះផលិតផល (ProductName) មកបង្ហាញជាមួយ
        //        string sql = @"SELECT rd.ReceiptId, rd.ProductId, p.ProductName, rd.Qty, rd.Price, rd.LineTotal 
        //               FROM ReceiptDetails rd 
        //               JOIN Products p ON rd.ProductId = p.ProductID 
        //               WHERE rd.ReceiptId = @ReceiptId";

        //        SqlCommand cmd = new SqlCommand(sql, con);
        //        cmd.Parameters.AddWithValue("@ReceiptId", receiptId);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ReceiptDetail detail = new ReceiptDetail
        //            {
        //                ReceiptId = Convert.ToInt32(reader["ReceiptId"]),
        //                ProductId = Convert.ToInt32(reader["ProductId"]),
        //                ProductName = reader["ProductName"].ToString(),
        //                Qty = Convert.ToInt32(reader["Qty"]),
        //                Price = Convert.ToDecimal(reader["Price"]),
        //                LineTotal = Convert.ToDecimal(reader["LineTotal"])
        //            };
        //            details.Add(detail);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error retrieving receipt details: " + ex.Message);
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return details;
        //}
    }
}