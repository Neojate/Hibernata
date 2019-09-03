using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{

    public class Hibernata<T> : INataDao<T>  where T : BaseModel
    {

        T obj = default(T);

        private List<PropertyInfo> properties = new List<PropertyInfo>();

        MySqlConnection connection = null;

        public Hibernata()
        {
            obj = Activator.CreateInstance<T>();
            properties = obj.Properties;
        }



        #region SELECT
        public T Select(object id)
        {
            return Select(new Filter(properties.First().Name, id).ToList());
        }

        public T Select(Filter filter)
        {
            return Select(filter.ToList());
        }

        public T Select(List<Filter> filters)
        {
            string sql =
                sentenceSqlFrom() +
                "WHERE " + separator(filters);

            return launchQueryAll(sql).FirstOrDefault(); 
        }
        #endregion



        #region SELECTALL
        public List<T> SelectAll()
        {
            return launchQueryAll(sentenceSqlFrom());
        }

        public List<T> SelectAll(Filter filter)
        {
            return SelectAll(filter.ToList());
        }

        public List<T> SelectAll(List<Filter> filters)
        {
            string sql =
                sentenceSqlFrom() +
                "WHERE " + separator(filters);

            return launchQueryAll(sql);
        }
        #endregion



        #region MÉTODOS PRIVADOS
        private MySqlDataReader createQuery(string sql)
        {
            connection = NataConnection.OpenConnection();
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(sql, connection);
            return cmd.ExecuteReader();
        }

        private void closeQuery()
        {
            if (connection != null)
                NataConnection.CloseConnection();
        }

        private string separator(List<string> objs)
        {
            string text = "";
            for (int i = 0; i < objs.Count - 1; i++)
                text += objs[i] + ", ";
            text += objs.Last();

            return text;
        }

        private string separator(List<Filter> objs)
        {
            string text = "";
            for (int i = 0; i < objs.Count - 1; i++)
                text += objs[i].ColumnName + " = '" + objs[i].ColumnValue + "' AND ";
            text += objs.Last().ColumnName + " = '" + objs.Last().ColumnValue + "'"; 
            return text;
        }

        private string sentenceSqlFrom()
        {
            return "SELECT * FROM " + obj.Name + " ";
        }

        private T launchQuery(string sql)
        {
            try
            {
                MySqlDataReader reader = createQuery(sql);
                if (reader.HasRows)
                    while (reader.Read())
                        obj = fillReadedObject(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                closeQuery();
            }
            return obj;
        }

        private List<T> launchQueryAll(string sql)
        {
            List<T> objs = new List<T>();
            try
            {
                MySqlDataReader reader = createQuery(sql);
                if (reader.HasRows)
                    while (reader.Read())
                        objs.Add(fillReadedObject(reader));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                closeQuery();
            }
            return objs;
        }

        private T fillReadedObject(MySqlDataReader reader)
        {
            T newObj = Activator.CreateInstance<T>();
            for (int i = 0; i < properties.Count; i++)
            {
                string typeName = properties[i].PropertyType.ToString();

                if (typeName.Contains("Int"))
                    properties[i].SetValue(newObj, reader.GetInt32(i));
                else if (typeName.Contains("Double"))
                    properties[i].SetValue(newObj, reader.GetDouble(i));
                else if (typeName.Contains("Decimal"))
                    properties[i].SetValue(newObj, reader.GetDecimal(i));
                else if (typeName.Contains("Float"))
                    properties[i].SetValue(newObj, reader.GetFloat(i));
                else if (typeName.Contains("String"))
                    properties[i].SetValue(newObj, reader.GetString(i));
                else if (typeName.Contains("Boolean"))
                    properties[i].SetValue(newObj, reader.GetBoolean(i));
                else if (typeName.Contains("DateTime"))
                    properties[i].SetValue(newObj, reader.GetDateTime(i));
            }
            return newObj;
        }
        #endregion

    }
}
