using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos
{
    public class DBConnection
    {
        protected static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString);
    }
}
