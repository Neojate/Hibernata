using System;
using Hibernata.Model;

namespace Hibernata.Model
{
    public class category : BaseModel
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }

        public category()
        {

        }

        public category(int categoryId, string categoryName)
        {
            this.categoryId = categoryId;
            this.categoryName = categoryName;
        }

    }
}
