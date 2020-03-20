using System.ComponentModel;

namespace Data.Enums
{
    public enum BookingStatus
    {
        [Description("Открыта")]
        Open = 1,

        [Description("Закрыта")]
        Close = 2
    }
}
