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
    public class UserDAL : DBConnection
    {
        public User Login(string username, string password)
        {
            string sql = @"SELECT UserId, Username, FullName, Role, IsActive 
                           FROM Users 
                           WHERE Username = @Username AND Password = @Password AND IsActive = 1";
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                if (con.State == ConnectionState.Closed) con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        User user = new User
                        {
                            Id = Convert.ToInt32(reader["UserId"]),
                            Username = reader["Username"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Role = reader["Role"].ToString(),
                            IsActive = true
                        };
                        con.Close();
                        return user; 
                    }
                }
                con.Close();
            }
            return null; 
        }
    }
}
