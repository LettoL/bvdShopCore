namespace Domain.Entities
{
    public class ScheduledDeliveryPayment : Entity
    {
        public int ScheduledDeliveryId { get; set; }
        public int InfoMoneyId { get; set; }
        public int MoneyWorkerId { get; set; }
    }
}