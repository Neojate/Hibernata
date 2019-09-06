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

            NataFactory entity = new NataFactory();

            entity.CreateBaseModel();

            /*INataDao<category> nataCategory = new Hibernata<category>();

            Console.WriteLine(nataCategory.Select(new Filter("categoryName", "Accion")).ToString());

            INataDao<platformuser> nataPlatformuser = new Hibernata<platformuser>();

            List<Filter> filters = new List<Filter>()
            {
                new Filter("lastname", "alonso"),
                new Filter("roleid", 2)
            };
            Console.WriteLine(nataPlatformuser.Select(filters).ToString());

            //foreach (var x in nataPlatformuser.SelectAll())
            //Console.WriteLine(x.ToString());

            List<platformuser> platformusers = new List<platformuser>()
            {
                new platformuser() { userID = "J", password = "K", firstname = "F", lastname = "G", mailAdress = "H", roleId = 2 },
                new platformuser() { userID = "X", password = "Y", firstname = "Z", lastname = "W", mailAdress = "V", roleId = 2 }
            };
            nataPlatformuser.Insert(platformusers);*/

            Console.WriteLine("Programa terminado");
            Console.ReadLine();
        }
    }
}
