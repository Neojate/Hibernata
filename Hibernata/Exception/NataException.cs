using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public class NataException : Exception
    {

        public static string NO_BRIDGED_CREATED = "Imposible conectarse. Todavía no se ha establecido el puente a la base de datos.";
        public static string NO_NULL_CREDENTIALS = "Las credenciales no pueden ser nulas.";

        public static string NO_FILTER_RECIPROCATION = "Los filtros establecidos no coinciden con los campos del BaseModel.";
        public static string NO_FIELD_RECIPROCATION = "Los campos establecidos no coinciden con los campos del BaseModel.";

        public static string INSERT_DUPLICATED_KEY = "Imposible insertar elemento. La clave primaria no puede duplicarse.";
        public static string INSERT_NO_FOREIGNKEY = "Imposible insertar elemento. Revise que la clave foranea exista.";

        public NataException(string message) : base(message)
        {

        }
    }
}
