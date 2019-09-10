using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hibernata.Model
{
    public class TableDefinition
    {
        public List<RowDefinition> Rows { get; set; }

        public TableDefinition()
        {
            Rows = new List<RowDefinition>();
        }

        public void SaveTableData(string nameFile)
        {
            string path = "Model\\" + nameFile + ".xml";

            StreamWriter file = new StreamWriter(path);
            XmlSerializer writer = new XmlSerializer(this.GetType());
            writer.Serialize(file, writer);
            file.Close();
        }

        public TableDefinition LoadTableData(string nameFile)
        {
            string path = "Model\\" + nameFile + ".xml";

            if (!File.Exists(path))
                return null;

            StreamReader file = new StreamReader(path);
            XmlSerializer reader = new XmlSerializer(this.GetType());
            TableDefinition tableDef = (TableDefinition)reader.Deserialize(file);
            file.Close();
            return tableDef;
        }

    }
}
