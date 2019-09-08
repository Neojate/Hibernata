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

        protected MySqlConnection connection = null;
        

        public NataFactory()
        {
            connection = NataConnection.OpenConnection();
        }

        public void CreateBaseModel(string namespaceName)
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

                createFileModel(namespaceName, s, columns);
            }
            
        }

        private void createFileModel(string namespaceName, string name, List<object[]> columns)
        {
            string dir = "Model";
            string fileName = name + ".txt";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            else
                if (!File.Exists(dir + "\\" + fileName))
                File.Delete(dir + "\\" + fileName);

            string body = "";

            body += "using System;\n";
            body += "using Hibernata.Model;\n\n";
            body += "namespace " + namespaceName + ".Model\n";
            body += "{\n";

            body += "\tpublic class " + name + " : BaseModel\n";
            body += "\t{\n";

            foreach (var c in columns)
                body += "\t\tpublic " + translateSqlType(c[1].ToString()) + " " + c[0].ToString() + " { get; set; }\n";

            body += "\n\t\tpublic " + name + "()\n";
            body += "\t\t{\n\n";
            body += "\t\t}\n\n";

            List<string> parameters = new List<string>();
            foreach (var c in columns)
                parameters.Add(translateSqlType(c[1].ToString()) + " " + c[0].ToString());
            body += "\t\tpublic " + name + "(" + separator(parameters) + ")\n";
            body += "\t\t{\n";
            foreach (var c in columns)
                body += "\t\t\tthis." + c[0].ToString() + " = " + c[0].ToString() + ";\n";
            body += "\t\t}\n\n";
            

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

        private string translateSqlType(string sql)
         {
            if (sql.Contains("tinyint(1)"))
                return "bool";

            if (sql.Contains("bigint"))
                return "long";
            if (sql.Contains("int("))
                return "int";
            if (sql.Contains("decimal"))
                return "decimal";
            if (sql.Contains("float"))
                return "float";
            if (sql.Contains("double"))
                return "double";

            if (sql.Contains("varchar(") || sql.Contains("text"))
                return "string";
            if (sql.Contains("char"))
                return "char";

            if (sql.Contains("date"))
                return "DateTime";

            throw new Exception("No se ha encontrado el tipo.");

        }


    }
}
