using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public class Persona : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string telf { get; set; }

        public Persona()
        {

        }

        public Persona(int iD, string name, string telf)
        {
            ID = iD;
            Name = name;
            this.telf = telf;
        }

    }
}
