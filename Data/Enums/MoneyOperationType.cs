using System.ComponentModel;

namespace Data.Enums
{
    public enum MoneyOperationType
    {
        [Description("Погашение долга перед поставщиком")]
        SupplierRepayment = 1,

        [Description("Инкассация")]
        Encashment = 2,

        [Description("Бронирование")]
        Booking = 3,

        [Description("Продажа")]
        Sale = 4,

        [Description("Расход")]
        Expense = 5,

        [Description("Перевод")]
        Transfer = 6,

        [Description("Пополнение")]
        Replenishment = 7
    }
}