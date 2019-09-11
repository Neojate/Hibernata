using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata.Model
{
    public class ForeignDefinition
    {
        public string TableAim { get; set; }
        public string ColumnAim { get; set; }
        public string UpdateRule { get; set; }
        public string DeleteRule { get; set; }

        public ForeignDefinition()
        {

        }

        public ForeignDefinition(string tableAim, string columnAim, string updateRule, string deleteRule)
        {
            TableAim = tableAim;
            ColumnAim = columnAim;
            UpdateRule = updateRule;
            DeleteRule = deleteRule;
        }


    }
}
