using System.Linq;
using Data.Entities;
using Data.Enums;
using PostgresData;

namespace Data.Services.Abstract
{
    public interface IMoneyOperationService
    {
        void SalePayment(ShopContext db, decimal cash, decimal cashless, Sale sale,
            int? moneyWorkerIdForCash, int? moneyWorkerIdForCashless);

        void Expense(PostgresContext postgresContext, int moneyWorkerId, decimal sum, PaymentType paymentType,
            int categoryId, string comment, int forId);

        void Expense(PostgresContext postgresContext, int moneyWorkerId, decimal sum, PaymentType paymentType,
            int categoryId, string comment, int shopId, int forId);

        PaymentType PaymentTypeByMoneyWorker(int moneyWorkerId);
    }
}