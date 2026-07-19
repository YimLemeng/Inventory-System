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
    public class CategoryDAL : DBConnection
    {
        public bool Save(Category cat)
        {
            try
            {
                string sql = "INSERT INTO Category (CategoryName, Description) VALUES (@CategoryName, @Description)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@CategoryName", cat.CategoryName);
                cmd.Parameters.AddWithValue("@Description", (object)cat.Description ?? DBNull.Value);
                if (con.State == ConnectionState.Closed) con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving category: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }
        public bool Update(Category cat)
        {
            try
            {
                string sql = "UPDATE Category SET CategoryName = @CategoryName, Description = @Description WHERE CategoryId = @CategoryId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@CategoryId", cat.Id);
                cmd.Parameters.AddWithValue("@CategoryName", cat.CategoryName);
                cmd.Parameters.AddWithValue("@Description", (object)cat.Description ?? DBNull.Value);
                if (con.State == ConnectionState.Closed) con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating category: " + ex.Message);
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
                string sql = "UPDATE Category SET IsActive = 0 WHERE CategoryId = @CategoryId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@CategoryId", id);
                if (con.State == ConnectionState.Closed) con.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        public DataSet GetAllCategory()
        {
            string sql = "SELECT * FROM Category WHERE IsActive = 1";
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
                throw new Exception("Error retrieving categories: " + ex.Message);
            }
        }
    }
}
