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
        private List<PropertyInfo> properties = new List<PropertyInfo>();

        public Hibernata()
        {
            properties = typeof(T).GetProperties().ToList();
        }

        public T Select(object id)
        {
            T obj = Activator.CreateInstance<T>();

            string sql =
                sentenceSqlFrom(obj) + 
                "WHERE " + properties.First().Name + " = " + id.ToString();

            return obj;
        }

        public T Select(Filter filter)
        {
            T obj = Activator.CreateInstance<T>();

            string sql =
                sentenceSqlFrom(obj) +
                "WHERE " + filter.ColumnName + " = " + filter.ColumnValue;

            return obj;
        }

        public T Select(List<Filter> filters)
        {
            T obj = Activator.CreateInstance<T>();

            string sql =
                sentenceSqlFrom(obj) +
                "WHERE " + separator(filters);

            return obj;
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
                text += objs[i].ColumnName + " = " + objs[i].ColumnValue + " AND ";
            text += objs.Last().ColumnName + " = " + objs.Last().ColumnValue; 
            return text;
        }

        private string sentenceSqlFrom(T obj)
        {
            return "SELECT * FROM " + obj.Name + " ";
        }

    }
}
