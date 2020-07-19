namespace Handlers.Commands
{
    public class CreateIncompleteProduct
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public int Amount { get; set; }
        public string Comment { get; set; }
    }
}