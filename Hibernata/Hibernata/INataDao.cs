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
        //Método que realiza una Query a través de una PrimaryKey única y devuelve toda las columnas.
        T Select(object id);

        //Método que realiza una Query a través de una PrimaryKey única y devuelve las columnas especificadas.
        T Select(string[] fields, object id);

        //Método que realiza una Query a través de un filtro único y devuelve todas las columnas.
        T Select(Filter filter);

        //Método que realiza una Query a través de un filtro único y devuelve las columnas especificadas.
        T Select(string[] fields, Filter filter);

        //Método que realiza una Query a través de múltiples filtros (AND) y devuelve todas las columnas.
        T Select(List<Filter> filters);

        //Método que realiza una Query a través de múltiples filtros (AND) y devuelve las columnas especificadas.
        T Select(string[] fields, List<Filter> filters);
        #endregion



        #region SELECTALL
        //Método que devuelve todas las filas de una tabla con todas sus columnas.
        List<T> SelectAll();

        //Método que devuelve todas las filas de una tabla con las columnas especificadas.
        List<T> SelectAll(string[] fields);

        //Método que devuelve todas las filas de una tabla según un filtro con todas sus columnas.
        List<T> SelectAll(Filter filter);

        //Método que devuelve todas las filas de una tabla según un filtro con las columnas especificadas.
        List<T> SelectAll(string[] fields, Filter filter);

        //Método que devuelve todas las filas de una tabla según una lista de filtros (AND) con todas sus columnas.
        List<T> SelectAll(List<Filter> filters);

        //Método que devuelve todas las filas de una tabla según una lista de filtros (AND) con las columnas especificadas.
        List<T> SelectAll(string[] fields, List<Filter> filters);
        #endregion



        #region INSERT
        //Método que inserta una fila entera en una tabla.
        int Insert(T obj);

        //Método que inserta de una fila las columnas especificadas en una tabla.
        int Insert(string[] fields, T obj);

        //Método que inserta muchas filas enteras en una tabla.
        int Insert(List<T> objs);

        //Método que inserta de muchas filas las columnas especificadas.
        int Insert(string[] fields, List<T> obj);
        #endregion



        #region UPDATE
        int Update(T obj);
        int Update(Filter set, Filter filter);
        int Update(Filter set, List<Filter> filters);
        int Update(List<Filter> sets, Filter filter);
        int Update(List<Filter> sets, List<Filter> filters);
        #endregion



        #region INSERT OR UPDATE
        int InsertOrUpdate(T obj);
        int InsertOrUpdate(List<T> objs);
        #endregion



        #region DELETE

        #endregion



        #region MISCELANEA

        #endregion


    }
}
