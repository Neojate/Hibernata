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



        #region UPDATE
        int Update(T obj);
        int Update(Filter set, Filter filter);
        int Update(Filter set, List<Filter> filters);
        int Update(List<Filter> sets, Filter filter);
        int Update(List<Filter> sets, List<Filter> filters);
        #endregion



        #region INSERT OR UPDATE

        #endregion



        #region DELETE

        #endregion



        #region MISCELANEA

        #endregion


    }
}
