using System;
using Hibernata.Model;

namespace Hibernata.Model
{
    public class platformuser : BaseModel
    {
        public string userID { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mailAddress { get; set; }
        public int roleId { get; set; }

        public platformuser()
        {

        }

        public platformuser(string userID, string password, string firstname, string lastname, string mailAddress, int roleId)
        {
            this.userID = userID;
            this.password = password;
            this.firstname = firstname;
            this.lastname = lastname;
            this.mailAddress = mailAddress;
            this.roleId = roleId;
        }

    }
}
