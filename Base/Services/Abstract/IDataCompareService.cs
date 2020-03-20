using System;

namespace Base.Services.Abstract
{
    public interface IDataCompareService
    {
        bool IsPeriod(DateTime value, DateTime? periodStart, DateTime? periodEnd);
    }
}