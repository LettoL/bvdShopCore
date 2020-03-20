using System.ComponentModel;

namespace WebUI.ViewModels
{
    public enum SalesByCategoriesFilterType
    {
        [Description("Магазин в Москве")]
        Moscow = 1,

        [Description("Магазин в Санкт-Петербурге")]
        Piter = 2,

        [Description("Продажи по России")]
        ForRF = 3,

        [Description("Оптовые продажи")]
        Partner = 4
    }
}