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

        public ForeignDefinition(string tableAim, string columnAim)
        {
            TableAim = tableAim;
            ColumnAim = columnAim;
        }

    }
}
