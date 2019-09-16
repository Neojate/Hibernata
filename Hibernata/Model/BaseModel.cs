using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata.Model
{
    public abstract class BaseModel
    {

        public override string ToString()
        {
            string text = "";
            foreach (var p in Properties)
                text += p.Name + ": " + p.GetValue(this) + "\n";
            return text;
        }

        public string Name
        {
            get { return GetType().Name.Split('.').Last(); }
        }

        private List<PropertyInfo> findPrimaryKeys()
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();

            string fileName = "Model\\" + Name + ".txt";
            if (!File.Exists(fileName))
                Console.WriteLine("Hacer que se lea desde aquí");

            TableDefinition tableDef = new TableDefinition().LoadTableData(Name);

            string[] primarykeysNames = tableDef.Rows
                .Where(x => TableDefinition.PRIMARY_KEY.Equals(x.Key))
                .Select(x => x.Field)
                .ToArray();

            foreach (string s in primarykeysNames)
                properties = Properties.Where(x => x.Name.Equals(s)).ToList();

            return properties;
        }

        public List<PropertyInfo> Properties
        {
            get { return GetType().GetProperties(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly
                ).ToList(); }
        }

        public List<string> PropertyNames
        {
            get { return Properties.Select(x => x.Name).ToList(); }
        }

        public List<PropertyInfo> PrimaryKeys
        {
            get { return findPrimaryKeys();  }
        }

        

    }
}
