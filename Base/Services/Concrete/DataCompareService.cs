using System;
using Base.Services.Abstract;

namespace Base.Services.Concrete
{
    public class DataCompareService : IDataCompareService
    {
        public bool IsPeriod(DateTime value, DateTime? periodStart, DateTime? periodEnd)
        {
            if ((periodStart == null || value >= periodStart) 
                    && (periodEnd == null || value <= periodEnd))
                return true;

            return false;
        }
    }
}