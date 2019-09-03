using MySql.Data.MySqlClient;
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
            NataConnection.CreateBridge("bdmarianflix", "albert", "");

            INataDao<category> nataCategory = new Hibernata<category>();

            Console.WriteLine(nataCategory.Select(new Filter("categoryName", "Accion")).ToString());

            INataDao<platformuser> nataPlatformuser = new Hibernata<platformuser>();

            List<Filter> filters = new List<Filter>()
            {
                new Filter("lastname", "alonso"),
                new Filter("roleid", 2)
            };
            Console.WriteLine(nataPlatformuser.Select(filters).ToString());

            foreach (var x in nataPlatformuser.SelectAll())
                //Console.WriteLine(x.ToString());

            Console.WriteLine("Programa terminado");
            Console.ReadLine();
        }
    }
}
