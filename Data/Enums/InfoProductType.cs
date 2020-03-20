using System.ComponentModel;

namespace Data.Enums
{
    public enum InfoProductType
    {
        [Description("Поставка")]
        Supply = 1,

        [Description("Списание")]
        Writeoff = 2,

        [Description("Перенос")]
        Transfer = 3
    }
}
