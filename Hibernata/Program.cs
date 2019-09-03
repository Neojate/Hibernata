using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    class Program
    {
        static void Main(string[] args)
        {
            INataDao<Persona> nata = new Hibernata<Persona>();

            nata.Select(1);
        }
    }
}
