using Hibernata.Model;
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

        private List<string> tables = new List<string>();
        

        public NataFactory()
        {
            connection = NataConnection.OpenConnection();
        }

        public void CreateBaseModel(string namespaceName)
        {
            MySqlDataReader reader = null;

            reader = createQuery("show tables");

            while (reader.Read())
                tables.Add(reader.GetString(0));

            closeQuery();

            foreach (string t in tables)
            {
                TableDefinition tableDef = new TableDefinition();

                tableDef.Rows = CreateRowDefinitionQuery(t);

                foreach (RowDefinition r in tableDef.Rows.Where(x => x.Key != null))
                    r.ForeignKey = createForeignDefinition(t, r.Field);

                tableDef.SaveTableData(t);

                createFileModel(namespaceName, t, tableDef.Rows);
            }            
        }

        public List<RowDefinition> CreateRowDefinitionQuery(string tableName)
        {
            List<RowDefinition> rows = new List<RowDefinition>();
            MySqlDataReader reader = createQuery("describe " + tableName);

            while (reader.Read())
            {
                RowDefinition row = new RowDefinition(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2).Equals("YES") ? true : false,
                    reader.GetString(3),
                    null,
                    reader.GetString(5).Contains("auto_increment") ? true : false,
                    null);
                rows.Add(row);
            }
            closeQuery();

            return rows;
        }

        private ForeignDefinition createForeignDefinition(string tableName, string columnName)
        {
            string sql =
                "SELECT U.REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME, UPDATE_RULE, DELETE_RULE " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE U " +
                "LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C " +
                "ON U.CONSTRAINT_NAME = C.CONSTRAINT_NAME " +
                "WHERE C.TABLE_NAME = '" + tableName + "' AND COLUMN_NAME = '" + columnName + "'";

            MySqlDataReader reader = createQuery(sql);

            ForeignDefinition foreignDef = null;

            while (reader.Read())
            {
                foreignDef = new ForeignDefinition(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3));
            }

            closeQuery();

            return foreignDef;
        } 



        private void createFileModel(string namespaceName, string name, List<RowDefinition> columns)
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
                body += "\t\tpublic " + translateSqlType(c.Type) + " " + c.Field + " { get; set; }\n";

            body += "\n\t\tpublic " + name + "()\n";
            body += "\t\t{\n\n";
            body += "\t\t}\n\n";

            List<string> parameters = new List<string>();
            foreach (var c in columns)
                parameters.Add(translateSqlType(c.Type) + " " + c.Field);
            body += "\t\tpublic " + name + "(" + separator(parameters) + ")\n";
            body += "\t\t{\n";
            foreach (var c in columns)
                body += "\t\t\tthis." + c.Field + " = " + c.Field + ";\n";
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
