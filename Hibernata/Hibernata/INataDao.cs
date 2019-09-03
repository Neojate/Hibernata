using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public interface INataDao<T>
    {
        T Select(object id);
        T Select(Filter filter);
        T Select(List<Filter> filters);

    }
}
