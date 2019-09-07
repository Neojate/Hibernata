using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata.Model
{
    public abstract class BaseModel
    {

        public List<BaseModel> ToList()
        {
            return new List<BaseModel>() { this };
        }

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

    }
}
