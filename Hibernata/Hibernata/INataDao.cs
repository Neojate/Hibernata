using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public interface INataDao<T>
    {

        #region SELECT
        T Select(object id);
        T Select(Filter filter);
        T Select(List<Filter> filters);
        #endregion



        #region SELECTALL
        List<T> SelectAll();
        List<T> SelectAll(Filter filter);
        List<T> SelectAll(List<Filter> filters);
        #endregion



        #region INSERT
        int Insert(T obj);

        int Insert(List<T> objs);
        #endregion

        List<string> ShowTables();

    }
}
