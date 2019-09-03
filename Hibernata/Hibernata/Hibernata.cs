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
            T obj = Activator.CreateInstance<T>();

            string sql =
                sentenceSqlFrom() + 
                "WHERE " + properties.First().Name + " = " + id.ToString();

            return launchQuery(sql);
        }

        public T Select(Filter filter)
        {
            T obj = Activator.CreateInstance<T>();

            string sql =
                sentenceSqlFrom() +
                "WHERE " + filter.ColumnName + " = '" + filter.ColumnValue + "'";

            return launchQuery(sql);
        }

        public T Select(List<Filter> filters)
        {
            T obj = Activator.CreateInstance<T>();

            string sql =
                sentenceSqlFrom() +
                "WHERE " + separator(filters);

            return launchQuery(sql); 
        }
        #endregion



        #region SELECTALL
        public List<T> SelectAll()
        {
            T obj = Activator.CreateInstance<T>();

            string sql = sentenceSqlFrom();

            return launchQueryAll(sql);
        }

        public List<T> SelectAll(Filter filter)
        {
            T obj = Activator.CreateInstance<T>();

            string sql =
                sentenceSqlFrom() +
                "WHERE " + filter.ColumnName + " = '" + filter.ColumnValue + "'";

            return launchQueryAll(sql);
        }

        public List<T> SelectAll(List<Filter> filters)
        {
            T obj = Activator.CreateInstance<T>();

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
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < properties.Count; i++)
                        {
                            string typeName = properties[i].PropertyType.ToString();

                            if (typeName.Contains("Int"))
                                properties[i].SetValue(obj, reader.GetInt32(i));
                            else if (typeName.Contains("Double"))
                                properties[i].SetValue(obj, reader.GetDouble(i));
                            else if (typeName.Contains("Decimal"))
                                properties[i].SetValue(obj, reader.GetDecimal(i));
                            else if (typeName.Contains("Float"))
                                properties[i].SetValue(obj, reader.GetFloat(i));
                            else if (typeName.Contains("String"))
                                properties[i].SetValue(obj, reader.GetString(i));
                            else if (typeName.Contains("Boolean"))
                                properties[i].SetValue(obj, reader.GetBoolean(i));
                            else if (typeName.Contains("DateTime"))
                                properties[i].SetValue(obj, reader.GetDateTime(i));
                        }
                    }
                }
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
                {
                    while (reader.Read())
                    {
                        T obj = Activator.CreateInstance<T>();
                        for (int i = 0; i < properties.Count; i++)
                        {
                            string typeName = properties[i].PropertyType.ToString();

                            if (typeName.Contains("Int"))
                                properties[i].SetValue(obj, reader.GetInt32(i));
                            else if (typeName.Contains("Double"))
                                properties[i].SetValue(obj, reader.GetDouble(i));
                            else if (typeName.Contains("Decimal"))
                                properties[i].SetValue(obj, reader.GetDecimal(i));
                            else if (typeName.Contains("Float"))
                                properties[i].SetValue(obj, reader.GetFloat(i));
                            else if (typeName.Contains("String"))
                                properties[i].SetValue(obj, reader.GetString(i));
                            else if (typeName.Contains("Boolean"))
                                properties[i].SetValue(obj, reader.GetBoolean(i));
                            else if (typeName.Contains("DateTime"))
                                properties[i].SetValue(obj, reader.GetDateTime(i));
                        }
                        objs.Add(obj);
                    }
                }
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
        #endregion

    }
}
