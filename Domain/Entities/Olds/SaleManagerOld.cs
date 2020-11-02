using System;

namespace Domain.Entities.Olds
{
    public class SaleManagerOld : Entity
    {
        public int ManagerId { get; private set; }
        public int SaleId { get; private set; }
        
        public DateTime SaleCreateDate { get; private set; }

        public SaleManagerOld(int managerId, int saleId, DateTime saleCreateDate) =>
            (ManagerId, SaleId, SaleCreateDate) = (managerId, saleId, saleCreateDate);
    }
}