using System.Collections.Generic;
using System.Linq;
using Data;
using PostgresData;
using WebUI.ViewModels;

namespace WebUI.QueriesHandlers
{
    public static class SupplierHandlers
    {
        public static ICollection<SupplierVM> Get(PostgresContext postgresContext, ShopContext shopContext)
        {
            var supplierInfos = postgresContext.SuppliersInfos
                .ToList();

            var removedSuppliers = supplierInfos
                .Where(x => x.Removed)
                .Select(x => x.SupplierId)
                .ToList();

            var result = shopContext.Suppliers
                .ToList()
                .Where(x => !removedSuppliers.Contains(x.Id))
                .Join(supplierInfos, 
                    s => s.Id,
                    i => i.SupplierId,
                    (s, i) => new
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Phone = s.Phone,
                        Email = s.Email,
                        Order = i.Order
                    })
                .OrderBy(x => x.Order)
                .Select(x => new SupplierVM()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Phone = x.Phone,
                    Email = x.Email,
                    Debt = 0,
                    CostRealizationProductOnStock = 0,
                    CostProductOnStock = 0
                }).ToList();

            return result;
        }
    }
}