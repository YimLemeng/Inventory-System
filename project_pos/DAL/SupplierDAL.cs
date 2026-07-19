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
    public class SupplierDAL : DBConnection
    {
        public bool Save(Supplier sup)
        {
            try
            {
                string sql = @"INSERT INTO Suppliers (SupplierName, ContactName, Phone, Email, Address) 
                           VALUES (@SupplierName, @ContactName, @Phone, @Email, @Address)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@SupplierName", sup.SupplierName);
                cmd.Parameters.AddWithValue("@ContactName", (object)sup.ContactName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", (object)sup.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)sup.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)sup.Address ?? DBNull.Value);
                if (con.State == ConnectionState.Closed) con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving supplier: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public bool Update(Supplier sup)
        {
            try
            {
                string sql = @"UPDATE Suppliers SET SupplierName = @SupplierName, ContactName = @ContactName, 
                           Phone = @Phone, Email = @Email, Address = @Address WHERE SupplierId = @SupplierId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@SupplierId", sup.Id);
                cmd.Parameters.AddWithValue("@SupplierName", sup.SupplierName);
                cmd.Parameters.AddWithValue("@ContactName", (object)sup.ContactName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Phone", (object)sup.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)sup.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)sup.Address ?? DBNull.Value);
                if (con.State == ConnectionState.Closed) con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating supplier: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public bool Delete(int id)
        {
            try
            {
                string sql = "UPDATE Suppliers SET IsActive = 0 WHERE SupplierId = @SupplierId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@SupplierId", id);
                if (con.State == ConnectionState.Closed) con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting supplier: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public DataSet GetAllSupplier()
        {
            string sql = "SELECT * FROM Suppliers WHERE IsActive = 1";
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
                throw new Exception("Error retrieving suppliers: " + ex.Message);
            }
        }
    }
}
