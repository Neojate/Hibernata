using Hibernata.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{

    public enum CrudType
    {
        Select, Insert
    }

    public class Hibernata<T> : PrimaryNataMethods, INataDao<T>  where T : BaseModel
    {

        T obj = default(T);

        private List<PropertyInfo> properties = new List<PropertyInfo>();

        MySqlConnection connection = null;

        public Hibernata()
        {
            obj = Activator.CreateInstance<T>();
            properties = obj.Properties;
            connection = NataConnection.OpenConnection();
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
            checkFilters(filters);

            string sql =
                sentenceSqlFrom(CrudType.Select) +
                "WHERE " + separator(filters);

            return launchQueryAll(sql).FirstOrDefault(); 
        }
        #endregion



        #region SELECTALL
        public List<T> SelectAll()
        {
            return launchQueryAll(sentenceSqlFrom(CrudType.Select));
        }

        public List<T> SelectAll(Filter filter)
        {
            return SelectAll(filter.ToList());
        }

        public List<T> SelectAll(List<Filter> filters)
        {
            checkFilters(filters);

            string sql =
                sentenceSqlFrom(CrudType.Select) +
                "WHERE " + separator(filters);

            return launchQueryAll(sql);
        }
        #endregion



        #region INSERT
        public int Insert(T obj)
        {
            TableDefinition tb = new TableDefinition().LoadTableData(obj.Name);

            string AIproperty = "";

            foreach (var p in tb.Rows.Where(x => x.IsAutoIncremental))
                AIproperty = p.Field;

            string sql = "INSERT INTO " + obj.Name + " (" + separator(obj.PropertyNames.Where(x => !x.Equals(AIproperty)).ToList()) + ") " +
                "VALUES (" + separator(obj.Properties.Where(x => !x.Name.Equals(AIproperty)).Select(x => "@" + x.Name).ToList()) + ")";

            return launchTransaction(sql, new List<T>() { obj });    
        }

        public int Insert(List<T> objs)
        {
            string sql = 
                sentenceSqlFrom(CrudType.Insert) +
                "VALUES ";
            for (int i = 0; i < objs.Count - 1; i++)
                sql += "(" + separator(properties.Select(x => "@" + x.Name).ToList()) + "), ";
            sql += "(" + separator(properties.Select(x => "@" + x.Name).ToList()) + ")";

            return launchTransaction(sql, objs);
        }
        #endregion
        


        #region MÉTODOS PRIVADOS
        private bool checkFilters(List<Filter> filters)
        {
            foreach (string f in filters.Select(x => x.ColumnName))
                if (properties.FirstOrDefault(p => p.Name.Equals(f, StringComparison.InvariantCultureIgnoreCase)) == null)
                    throw new NataException(NataException.NO_FILTER_RECIPROCATION);
            return true;   
        }

        private string sentenceSqlFrom(CrudType type)
        {
            string sqlBegin = "";
            switch(type)
            {
                case CrudType.Select:
                    sqlBegin = "SELECT * FROM ";
                    break;
                case CrudType.Insert:
                    sqlBegin = "INSERT INTO ";
                    break;
            }
            return sqlBegin + obj.Name + " ";
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
        
        private int launchTransaction(string sql, List<T> objs)
        {
            int result = 0;
            MySqlTransaction tx = null;

            try
            {
                MySqlCommand cmd = createTransaction(sql);
                tx = connection.BeginTransaction();

                foreach (var o in objs)
                    foreach (var p in properties)
                        cmd.Parameters.AddWithValue("@" + p.Name, p.GetValue(o));

                result = cmd.ExecuteNonQuery();
                tx.Commit();
            }
            catch (MySqlException e)
            {
                tx.Rollback();
                throw new NataException(GetNataErrorMessage(e.Number));
            }

            catch (Exception e)
            {
                if (result != objs.Count)
                    tx.Rollback();
                Console.WriteLine(e);
            }
            finally
            {
                closeQuery();
            }
            return result;
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
