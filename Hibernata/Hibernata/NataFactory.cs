using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public class NataFactory : PrimaryNataMethods
    {

        MySqlConnection connection = null; 

        public NataFactory()
        {
            connection = NataConnection.OpenConnection();
        }

        public void CreateBaseModel()
        {
            List<string> tables = new List<string>();
            MySqlDataReader reader = null;

            reader = createQuery("show tables");

            while (reader.Read())
                tables.Add(reader.GetString(0));

            closeQuery();

            foreach (string s in tables)
            {
                List<object[]> columns = new List<object[]>();

                reader = createQuery("describe " + s);
                while (reader.Read())
                    columns.Add(new object[] { reader.GetString(0), reader.GetString(1) });
 
                closeQuery();

                createFileModel(s, columns);
            }
            
        }

        private void createFileModel(string name, List<object[]> columns)
        {
            string dir = "Model";
            string fileName = name + ".txt";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            else
                if (!File.Exists(dir + "\\" + fileName))
                    File.Delete(dir + "\\" + fileName);

            string body = "";

            body += "using Hibernata\n\n";
            body += "namespace Model\n";
            body += "{\n";

            body += "\tpublic class " + name + " : BaseModel\n";
            body += "\t{\n";

            foreach (var c in columns)
                body += "\t\tpublic " + c[1].ToString() + " " + c[0].ToString() + " { get; set; }\n"; 

            body += "\t}\n";

            body += "}\n";

            string path = dir + "\\" + name + ".txt";
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] text = new UTF8Encoding(true).GetBytes(body);
                    fs.Write(text, 0, text.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }
}
