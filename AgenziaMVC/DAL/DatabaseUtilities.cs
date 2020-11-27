using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace AgenziaMVC.DAL
{
    public static class DatabaseUtilities
    {
        public static SqlConnection OpenSession()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                // var connectionString = ConfigurationManager.ConnectionStrings["WingtipToys"].ConnectionString;
                SqlConnection conn = new SqlConnection { ConnectionString = connectionString };
                conn.Open();
                return conn;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}