using System.Collections.Generic;
using Base;

namespace Data.Entities
{
    public class Partner : BaseObject
    {
        public string Title { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public ICollection<PartnerProduct> PartnersProducts { get; set; }
    }
}
