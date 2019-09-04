using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public class platformuser : BaseModel
    {
        public string userID { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mailAdress { get; set; }
        public int roleId { get; set; }
    }
}
