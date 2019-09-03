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

            INataDao<category> nataDao = new Hibernata<category>();
            Console.WriteLine(nataDao.Select(1).Name);

            Console.WriteLine("Programa terminado");
            Console.ReadLine();
        }
    }
}
