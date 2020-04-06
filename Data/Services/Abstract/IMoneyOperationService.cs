using System.Linq;
using Data.Entities;
using Data.Enums;

namespace Data.Services.Abstract
{
    public interface IMoneyOperationService
    {
        void SalePayment(ShopContext db, decimal cash, decimal cashless, Sale sale,
            int? moneyWorkerIdForCash, int? moneyWorkerIdForCashless);

        void Expense(int moneyWorkerId, decimal sum, PaymentType paymentType,
            int categoryId, string comment);

        void Expense(int moneyWorkerId, decimal sum, PaymentType paymentType,
            int categoryId, string comment, int shopId);

        PaymentType PaymentTypeByMoneyWorker(int moneyWorkerId);
    }
}