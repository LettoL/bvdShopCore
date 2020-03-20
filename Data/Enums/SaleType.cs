using System.ComponentModel;

namespace Data.Enums
{
    public enum SaleType
    {
        [Description("Обычная продажа")]
        Sale = 0,

        [Description("Продажа со склада")]
        SaleFromStock = 1,

        [Description("Продажа с отложенным платежом")]
        DefferedSale = 2,

        [Description("Отложенная продажа со склада поставщика")]
        DefferedSaleFromStock = 3,

        [Description("Бронирование")]
        Booking = 4
    }
}