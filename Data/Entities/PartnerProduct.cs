using Base;

namespace Data.Entities
{
    public class PartnerProduct : BaseObject
    {
        public int PartnerId { get; set; }
        public Partner Partner { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ProductAmount { get; set; }
    }
}
