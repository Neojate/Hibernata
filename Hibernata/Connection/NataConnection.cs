using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hibernata
{
    public enum ConnectionType
    {
        None, MySQL
    }

    public class NataConnection
    {
        private ConnectionType connectionType;
        private string url;
        private string user;
        private string password;

        public NataConnection()
        {
            connectionType = ConnectionType.None;
        }

        public NataConnection(string url) : this()
        {
            this.url = url;
        }

        public NataConnection(string url, string user) : this(url)
        {
            this.user = user;
        }

        public NataConnection(string url, string user, string password) : this(url, user)
        {
            this.password = password;
        }

        public void CreateBridge(ConnectionType connectionType) 
        {
            switch(connectionType)
            {
                case ConnectionType.MySQL:
                    break;
            }
        }

        public void OpenConnection()
        {
            if (connectionType == ConnectionType.None)
                throw new NataException(NataException.NO_TYPE_CONNECTION);
        }

    }
}
