using System;

namespace Domain.Entities.Olds
{
    public class ManagerPayment : Entity
    {
        public int ManagerId { get; private set; }

        public DateTime DateTime { get; private set; }

        public decimal Sum { get; private set; }

        public string Comment { get; private set; }

        public int InfoMoneyId { get; private set; }

        public ManagerPayment(int managerId, DateTime dateTime, decimal sum, string comment, int infoMoneyId) =>
            (ManagerId, DateTime, Sum, Comment, InfoMoneyId) = (managerId, dateTime, sum, comment, infoMoneyId);
    }
}