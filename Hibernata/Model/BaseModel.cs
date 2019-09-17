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
        private TableDefinition tableDef = new TableDefinition();

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



        #region MÉTODOS PRIVADOS
        private void createXMLIfNotExists()
        {
            string fileName = "Model\\" + Name + ".xml";
            if (File.Exists(fileName))
                return;

            NataFactory factory = new NataFactory();
            TableDefinition tableDef = new TableDefinition();
            tableDef.Rows = factory.CreateRowDefinitionQuery(Name);
            tableDef.SaveTableData(Name);
        }

        private List<PropertyInfo> findPrimaryKeys()
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();

            createXMLIfNotExists();

            TableDefinition tableDef = new TableDefinition().LoadTableData(Name);

            string[] primarykeysNames = tableDef.Rows
                .Where(x => TableDefinition.PRIMARY_KEY.Equals(x.Key))
                .Select(x => x.Field)
                .ToArray();

            foreach (string s in primarykeysNames)
                properties = Properties.Where(x => x.Name.Equals(s)).ToList();

            return properties;
        }

        private List<PropertyInfo> findAutoIncremental()
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();

            createXMLIfNotExists();

            tableDef = tableDef.LoadTableData(Name);

            string[] AINames = tableDef.Rows
                .Where(x => x.IsAutoIncremental)
                .Select(x => x.Field)
                .ToArray();

            foreach (string s in AINames)
                properties = Properties.Where(x => x.Name.Equals(s)).ToList();

            return properties;
        }
        #endregion

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

        public List<PropertyInfo> PrimaryKeyFields
        {
            get { return findPrimaryKeys();  }
        }

        public List<PropertyInfo> IncrementalFields
        {
            get { return findAutoIncremental(); }
        }

        

    }
}
