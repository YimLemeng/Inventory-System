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
    public class CustomerDAL : DBConnection
    {
        public DataSet GatAllCustomer()
        {
            string sql = "SELECT * FROM Customers";
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
                throw new Exception("Error retrieving payment methods: " + ex.Message);
            }
        }
    }
}
