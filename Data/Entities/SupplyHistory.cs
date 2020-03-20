using System.Collections.Generic;
using Base;

namespace Data.Entities
{
    public class SupplyHistory : BaseObject
    {
        public ICollection<SupplyProduct> SupplyProducts { get; set; }

        public ICollection<InfoProduct> InfoProducts { get; set; }
    }
}