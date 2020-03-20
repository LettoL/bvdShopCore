using System.ComponentModel;

namespace Data.Enums
{
    public enum SupplyType
    {
        [Description("Для продажи")]
        ForSale = 1,

        [Description("Для реализации")]
        ForRealization = 2,

        [Description("С отложенным платежом")]
        DeferredPayment = 3
    }
}