using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public class NataConnection
    {

        private static MySqlConnection connection = null;

        public static string Url { get; set; }
        public static string Port { get; set; }
        public static string DataBaseName { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string StringConnection { get; private set; }


        private NataConnection()
        {
            
        }


        public static void CreateBridge(string databaseName, string userName, string password)
        {
            Url = "server=127.0.0.1;";
            Port = "port=3306;";
            DataBaseName = "database=" + databaseName + ";";
            UserName = "username=" + userName + ";";
            Password = "password=" + password + ";";
        }

        public static void CreateBridge(string url, string port, string databaseName, string userName, string password)
        {
            CreateBridge(databaseName, userName, password);
            Url = "server=" + url + ";";
            Port = "port=" + port + ";";
        }

        public static MySqlConnection OpenConnection()
        {
            if (Url == null)
                throw new NataException(NataException.NO_BRIDGED_CREATED);

            StringConnection = Url + Port + UserName + Password + DataBaseName;

            if (connection == null)
                connection = new MySqlConnection(StringConnection);

            return connection;
        }

        public static void CloseConnection()
        {
            if (connection != null)
                connection.Close();
        }

    }
}
