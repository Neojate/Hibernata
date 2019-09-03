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

        public NataException(string message) : base(message)
        {

        }
    }
}
