using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public class PrimaryNataMethods
    {

        private MySqlConnection connection = null;

        public PrimaryNataMethods()
        {
            connection = NataConnection.OpenConnection();
        }

        protected MySqlDataReader createQuery(string sql)
        {
            tryOpenConnection();
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            return cmd.ExecuteReader();
        }

        protected MySqlCommand createTransaction(string sql)
        {
            tryOpenConnection();
            return new MySqlCommand(sql, connection);
        }

        protected void closeQuery()
        {
            if (connection != null && connection.State == ConnectionState.Open)
                connection.Close();
        }

        private void tryOpenConnection()
        {
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Error a la hora de abrir la conexión.");
            }
        }
        

    }
}
