using System.ComponentModel;

namespace Data.Enums
{
    public enum PaymentType
    {
        [Description("Наличный расчёт")]
        Cash = 1,

        [Description("Безналичный расчёт")]
        Cashless = 2,

        [Description("Смешанный расчёт")]
        Mixed = 3
    }
}
