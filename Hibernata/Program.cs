﻿using Hibernata.Model;
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

            NataFactory factory = new NataFactory();
            INataDao<category> nataCategory = new Hibernata<category>();
            INataDao<platformuser> nataUser = new Hibernata<platformuser>();
            INataDao<watchedlist> nataWatched = new Hibernata<watchedlist>();

            try
            {
                //factory.CreateBaseModel("Hibernata");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                category c = new category(3, "drama");
                nataCategory.InsertOrUpdate(c);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            /*try
            {
                List<Filter> sets = new List<Filter>()
                {
                    new Filter("lastname", "Apellido")
                };
                Filter filter = new Filter("roleId", 2);
                nataUser.Update(sets, filter);
            }
            catch (Exception e)
            {

            }*/

            /*INataDao<category> nataCategory = new Hibernata<category>();
            try
            {
                nataCategory.Update(new category(6, "Zombis"));
                List<category> categories = new List<category>()
                {
                    new category(1, "Misterio"),
                    new category(1, "Ciencia ficción"),
                    new category(1, "Slasher")
                };
                nataCategory.Insert(new category(1, "Marujeo"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            /*INataDao<watchedlist> nataWatched = new Hibernata<watchedlist>();

            try
            {
                watchedlist w = new watchedlist(1, "Alonsus", 5, new DateTime(), 5);
                nataWatched.Insert(w);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            INataDao<platformuser> nataPlatform = new Hibernata<platformuser>();

            try
            {
                platformuser p = new platformuser("CuentaUsuario", "aaaa", "Uno", "Dosdos", "admjkin@admin.com", 2);
                //nataPlatform.Insert(p);

                Console.WriteLine(nataPlatform.Select(new Filter("passwordx", "aaaa")).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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

            Console.WriteLine("Programa terminado.");
            Console.ReadLine();
        }
    }
}
