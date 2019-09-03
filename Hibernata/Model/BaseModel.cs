using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public abstract class BaseModel
    {

        public string Name
        {
            get { return typeof(BaseModel).Name.Split('.').Last(); }
        }

        public List<string> PropertyNames
        {
            get { return typeof(BaseModel).GetProperties().Select(x => x.Name).ToList(); }
        }
    }
}
