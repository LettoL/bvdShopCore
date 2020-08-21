using System;

namespace Domain.Entities
{
    public class DepositedMoney
    {
        public Guid Id { get; private set; }

        public Guid AccountId { get; private set; }

        public decimal Sum { get; private set; }

        public DateTime DateTime { get; private set; }

        public DepositedMoney(Guid id, Guid accountId, decimal sum, DateTime dateTime)
        {
            Id = id;
            AccountId = accountId;
            Sum = sum;
            DateTime = dateTime;
        }
        
        private DepositedMoney() {}
    }
}