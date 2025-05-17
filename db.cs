using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PUTINBD
{
     class db
    {
        SqlConnection SqlConnection = new SqlConnection(@"Data Source=PILOTPC\SQLEXPRESS;Initial Catalog=DBPUTIN;Integrated Security=True;Encrypt=False");

        public void openConnection()
        {

            if (SqlConnection.State == System.Data.ConnectionState.Closed)
            {

                SqlConnection.Open();
            }
        }
        public void closeConnection()
        {

            if (SqlConnection.State == System.Data.ConnectionState.Open)
            {

                SqlConnection.Close();
            }
        }
        public SqlConnection GetSqlConnection()
        {
            return SqlConnection;
        }
    }
}
