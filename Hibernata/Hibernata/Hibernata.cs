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

        private const string PRIMARY_KEY = "PRI";

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
                "WHERE " + separator(filters, Filter.AND);

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
                "WHERE " + separator(filters, Filter.AND);

            return launchQueryAll(sql);
        }
        #endregion



        #region INSERT
        public int Insert(T obj)
        {
            return Insert(new List<T>() { obj });    
        }

        public int Insert(List<T> objs)
        {
            if (objs.Count <= 0)
                return -1;

            TableDefinition tb = new TableDefinition().LoadTableData(objs.First().Name);

            string AIproperty = "";

            foreach (var p in tb.Rows.Where(x => x.IsAutoIncremental))
                AIproperty = p.Field;

            string sql = sentenceSqlFrom(CrudType.Insert) + obj.Name + " " +
                "(" + separator(obj.PropertyNames.Where(x => !x.Equals(AIproperty)).ToList()) + ") VALUES ";
            for (int i = 0; i < objs.Count - 1; i++)
                sql += "(" + separator(objs[i].Properties.Where(x => !x.Name.Equals(AIproperty)).Select(x => x.GetValue(objs[i]).ToString()).ToList(), "'") + "), ";
            sql += "(" + separator(objs.Last().Properties.Where(x => !x.Name.Equals(AIproperty)).Select(x => x.GetValue(objs.Last()).ToString()).ToList(), "'") + ")";
            
            return launchTransaction(sql);
        }
        #endregion



        #region UPDATE
        public int Update(T obj)
        {
            List<PropertyInfo> nonKeys = obj.Properties;
            foreach (var p in obj.PrimaryKeyFields)
                nonKeys.Remove(p);

            string sql = 
                "UPDATE " + obj.Name + " " +
                "SET ";
            for (int i = 0; i < nonKeys.Count - 1; i++)
                sql += nonKeys[i].Name +  " = '" + nonKeys[i].GetValue(obj) + "', ";
            sql += nonKeys.Last().Name + " = '" + nonKeys.Last().GetValue(obj) + "' ";
            sql += "WHERE ";
            for (int i = 0; i < obj.PrimaryKeyFields.Count - 1; i++)
                sql += obj.PrimaryKeyFields[i].Name + " = '" + obj.PrimaryKeyFields[i].GetValue(obj) + "' AND ";
            sql += obj.PrimaryKeyFields.Last().Name + " = '" + obj.PrimaryKeyFields.Last().GetValue(obj) + "'";

            return launchTransaction(sql);
        }

        public int Update(Filter set, Filter filter)
        {
            return Update(set.ToList(), filter.ToList());
        }

        public int Update(Filter set, List<Filter> filters)
        {
            return Update(set.ToList(), filters);
        }

        public int Update(List<Filter> sets, Filter filter)
        {
            return Update(sets, filter.ToList());
        }

        public int Update(List<Filter> sets, List<Filter> filters)
        {
            List<PropertyInfo> nonKeys = obj.Properties;
            foreach (var p in obj.PrimaryKeyFields)
                nonKeys.Remove(p);

            string sql =
                "UPDATE " + obj.Name + " " +
                "SET " + separator(sets, Filter.COMA) + " " +
                "WHERE " + separator(filters, Filter.AND);

            return launchTransaction(sql);
        }
        #endregion



        #region INSERT OR UPDATE
        public int InsertOrUpdate(T obj)
        {
            List<PropertyInfo> ps = obj.Properties;

            foreach (var pK in obj.PrimaryKeyFields)
                ps.Remove(pK);

            foreach (var iA in obj.IncrementalFields)
                ps.Remove(iA);

            List<string> updatefields = new List<string>();
            foreach (var p in ps)
                updatefields.Add(p.Name + " = " + p.GetValue(obj).ToString());

            string sql =
                sentenceSqlFrom(CrudType.Insert) +
                "VALUES (";
            for (int i = 0; i < obj.Properties.Count - 1; i++)
                sql += "'" + obj.Properties[i].GetValue(obj) + "', ";
            sql += "'" + obj.Properties.Last().GetValue(obj) + "') ";
            sql += "ON DUPLICATE KEY UPDATE ";
            for (int i = 0; i < ps.Count - 1; i++)
                sql += ps[i].Name + " = '" + ps[i].GetValue(obj).ToString() + "', ";
            sql += ps.Last().Name + " = '" + ps.Last().GetValue(obj).ToString() + "'";

            return launchTransaction(sql);

            //hay que rehacer los separators
        }

        public int InsertOrUpdate(List<T> obj)
        {
            return 0;
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

        private int launchTransaction(string sql)
        {
            int result = 0;
            MySqlTransaction tx = null;

            try
            {
                MySqlCommand cmd = createTransaction(sql);
                tx = connection.BeginTransaction();

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
