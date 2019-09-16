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

        private Dictionary<int, string> nataCodeErrors = new Dictionary<int, string>()
        {
            { 1062, NataException.INSERT_DUPLICATED_KEY },
            { 1452, NataException.INSERT_NO_FOREIGNKEY }
        };

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

        protected string GetNataErrorMessage(int i)
        {
            return nataCodeErrors[i];
        }

        protected string separator(List<string> objs)
        {
            string text = "";
            for (int i = 0; i < objs.Count - 1; i++)
                text += objs[i] + ", ";
            text += objs.Last();

            return text;
        }

        protected string separator(List<string> objs, string symbol)
        {
            string text = "";
            for (int i = 0; i < objs.Count - 1; i++)
                text +=  symbol + objs[i] + symbol + ", ";
            text += symbol + objs.Last() + symbol;
            return text;
        }

        protected string separator(List<Filter> objs)
        {
            string text = "";
            for (int i = 0; i < objs.Count - 1; i++)
                text += objs[i].ColumnName + " = '" + objs[i].ColumnValue + "' AND ";
            text += objs.Last().ColumnName + " = '" + objs.Last().ColumnValue + "'";
            return text;
        }

        private void tryOpenConnection()
        {
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error a la hora de abrir la conexión.");
            }
        }

        protected void readXml(string fileName)
        {

        }




    }
}
