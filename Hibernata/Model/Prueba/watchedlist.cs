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
    }
}
