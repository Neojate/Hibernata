using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public class Filter
    {
        private string columnValue;

        public Filter(string ColumnName, object columnValue)
        {
            this.ColumnName = ColumnName;
            this.columnValue = columnValue.ToString();
        }

        public string ColumnName
        {
            get { return ColumnName; }
            set { ColumnName = value; }
        }

        public object ColumnValue
        {
            get { return columnValue.ToString(); }
            set { columnValue = value.ToString(); }
        }

    }
}
