using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata.Model
{
    public class RowDefinition
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public bool IsNullable { get; set; }
        public string Key { get; set; }
        public string DefaultValue { get; set; }
        public bool IsAutoIncremental { get; set; }
        public ForeignDefinition ForeignKey { get; set; }

        public RowDefinition(string field, string type, bool isNullable, string key, string defaultValue, bool isAutoIncremental, ForeignDefinition foreignKey)
        {
            Field = field;
            Type = type;
            IsNullable = isNullable;
            Key = key;
            DefaultValue = defaultValue;
            IsAutoIncremental = isAutoIncremental;
            ForeignKey = foreignKey;
        }

    }
}
