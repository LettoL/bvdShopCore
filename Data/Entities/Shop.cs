using System.Collections.Generic;
using Base;

namespace Data.Entities
{
    public class Shop : MoneyWorker
    {
        public ICollection<Product> Products { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}
