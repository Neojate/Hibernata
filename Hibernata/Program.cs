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

            foreach (var x in nataPlatformuser.SelectAll(new List<Filter>(){
                new Filter("lastname", "Alonso"),
                new Filter("roleId", 2) } ))
                Console.WriteLine(x.ToString());

            Console.WriteLine("Programa terminado");
            Console.ReadLine();
        }
    }
}
