using Base;

namespace Data.Entities
{
    public abstract class MoneyWorker : BaseObject
    {
        public string Title { get; set; }

        public string Descriminator { get; }
    }
}