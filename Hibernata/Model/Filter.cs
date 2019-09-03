using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public class Filter
    {
        private string columnName;
        private string columnValue;

        public Filter(string columnName, object columnValue)
        {
            ColumnName = columnName;
            ColumnValue = columnValue;
        }

        public List<Filter> ToList()
        {
            return new List<Filter>() { this };
        }

        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        public object ColumnValue
        {
            get { return columnValue.ToString(); }
            set { columnValue = value.ToString(); }
        }

    }
}
