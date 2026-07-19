using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.DAL
{
    public class PaymentMethodDAL : DBConnection
    {
        public DataSet GetAllPaymentMethod()
        {
            string sql = "SELECT * FROM PaymentMethods";
            SqlDataAdapter da = new SqlDataAdapter(sql, con);
            DataSet ds = new DataSet();

            try
            {
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving payment methods: " + ex.Message);
            }
        }
    }
}
