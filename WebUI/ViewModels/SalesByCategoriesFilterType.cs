using System.ComponentModel;

namespace WebUI.ViewModels
{
    public enum SalesByCategoriesFilterType
    {
        [Description("Магазин в Москве")]
        Moscow = 1,

        [Description("Магазин в Санкт-Петербурге")]
        Piter = 2,
        
        [Description("Магазин в Самаре")]
        Samara = 27,
        
        [Description("Магазин Москва Север")]
        MoscowSever = 29,
        
        [Description("Магазин в Екатеринбурге")]
        Yekaterinburg = 33,

        [Description("Продажи по России")]
        ForRF = 3,

        [Description("Оптовые продажи")]
        Partner = 4
    }
}