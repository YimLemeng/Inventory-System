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
    public class ProductDAL : DBConnection
    {
        public DataSet GetAllProduct()
        {
            string sql = @"SELECT p.ProductID, p.ProductName, p.Barcode, p.CategoryId, 
                                  c.CategoryName, p.UnitPrice, p.StockQty, p.IsActive
                           FROM Products p
                           LEFT JOIN Category c ON p.CategoryId = c.CategoryId
                           WHERE p.IsActive = 1";
            DataSet ds = new DataSet();

            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, con))
                {
                    da.Fill(ds);
                }
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving products: " + ex.Message);
            }
        }
        public bool Save(Product p)
        {
            string sql = "INSERT INTO Products (ProductName, Barcode, CategoryId, UnitPrice, StockQty) VALUES (@Name, @Barcode, @CategoryId, @Price, @StockQty)";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("Name", p.ProductName);
            cmd.Parameters.AddWithValue("Barcode", p.Barcode);
            cmd.Parameters.AddWithValue("CategoryId", p.CategoryId);
            cmd.Parameters.AddWithValue("Price", p.UnitPrice);
            cmd.Parameters.AddWithValue("StockQty", p.StockQty);

            con.Open();
            int rows = cmd.ExecuteNonQuery();
            con.Close();
            return rows > 0;
        }

        public bool Update(Product p)
        {
            string sql = "UPDATE Products SET ProductName=@Name, Barcode=@Barcode, CategoryId=@CategoryId, UnitPrice=@Price, StockQty=@StockQty WHERE ProductId=@Id";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("Id", p.ProductID);
            cmd.Parameters.AddWithValue("Name", p.ProductName);
            cmd.Parameters.AddWithValue("Barcode", p.Barcode);
            cmd.Parameters.AddWithValue("CategoryId", p.CategoryId);
            cmd.Parameters.AddWithValue("Price", p.UnitPrice);
            cmd.Parameters.AddWithValue("StockQty", p.StockQty);
            con.Open();
            int rows = cmd.ExecuteNonQuery();
            con.Close();
            return rows > 0;
        }

        public bool Delete(int id)
        {
            string sql = "UPDATE Products SET IsActive = 0 WHERE ProductId = @Id";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("Id", id);
            con.Open();
            int rows = cmd.ExecuteNonQuery();
            con.Close();
            return rows > 0;
        }
        public void ReduceStock(int productId, int qty)
        {
            string sql = "UPDATE Products SET StockQty = StockQty - @Qty WHERE ProductId = @ProductId";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Qty", qty);
            cmd.Parameters.AddWithValue("@ProductId", productId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
