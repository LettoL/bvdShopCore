using System.Collections.Generic;
using Base;

namespace Data.Entities
{
    public class Category : BaseObject
    {
        public string Title { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}