using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata.Connection
{
    public class DBConnection
    {

        private static MySqlConnection connection = null;

        public string Url { get; set; }
        public string DataBaseName { get; set; }
        public string UserName { get; set; }
        public static string Password { get; set; }

        private static string stringConnection;


        private DBConnection()
        {

        }

        public static void CreateBridge(string databaseName, string userName, string password)
        {
            if (databaseName == null || userName == null || password == null)
                throw new NataException(NataException.NO_NULL_CREDENTIALS);

            stringConnection =
                "Server=localhost; " +
                "database=" + databaseName + "; " +
                "username=" + userName + "; " +
                "password=" + password;
        }

        public static void CreateBridge(string url, string databaseName, string userName, string password)
        {
            if (url == null || databaseName == null || userName == null || password == null)
                throw new NataException(NataException.NO_NULL_CREDENTIALS);

            stringConnection =
                "Server=" + url + "; " +
                "database=" + databaseName + "; " +
                "username=" + userName + "; " +
                "password=" + password;
        }

        public static MySqlConnection OpenConnection()
        {
            if (stringConnection == null)
                throw new NataException(NataException.NO_BRIDGED_CREATED);

            if (connection == null)
                connection = new MySqlConnection(stringConnection);
            return connection;
        }

        public static void CloseConnection()
        {
            if (connection != null)
                connection.Close();
        }

    }
}
