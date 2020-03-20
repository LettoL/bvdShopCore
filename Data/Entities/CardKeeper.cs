namespace Data.Entities
{
    public class CardKeeper : MoneyWorker
    {
        public string CardNumber { get; set; }
        public bool ForManager { get; set; }
    }
}