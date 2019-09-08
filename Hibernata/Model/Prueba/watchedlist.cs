using System;
using Hibernata.Model;

namespace Hibernata.Model
{
    public class watchedlist : BaseModel
    {
        public int itemID { get; set; }
        public string userID { get; set; }
        public int filmID { get; set; }
        public DateTime data { get; set; }
        public int starsNumber { get; set; }

        public watchedlist()
        {

        }

        public watchedlist(int itemID, string userID, int filmID, DateTime data, int starsNumber)
        {
            this.itemID = itemID;
            this.userID = userID;
            this.filmID = filmID;
            this.data = data;
            this.starsNumber = starsNumber;
        }

    }
}
