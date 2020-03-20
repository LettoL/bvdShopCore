using System;
using System.Linq;
using Data.Entities;
using Data.ModernServices.Abstract;
using Data.ViewModels;

namespace Data.ModernServices.Concrete
{
    public class SalesByCategoryService : ISalesByCategoryService
    {
        private readonly ShopContext _context;

        public SalesByCategoryService(ShopContext context)
        {
            _context = context;
        }

        public IQueryable<SaleByCategoryVM> SaleByCategory(IQueryable<SaleProduct> saleProducts,
            IQueryable<ProductInformation> productInformations)
        {
            return _context.Categories
                .Select(x => new SaleByCategoryVM()
                {
                    Category = x,
                    SalesByMoscow = ProductSalesByShop(
                        SaleProductsByCategory(saleProducts, x.Id), 1),
                    SalesByPetersburg = ProductSalesByShop(
                        SaleProductsByCategory(saleProducts, x.Id), 2),
                    Margin = Math.Round(
                        IncomeBySales(
                            SaleProductsByCategory(saleProducts, x.Id))
                        - PurchaseProductPrice(
                            ProductInformationsByCategory(productInformations, x.Id)),
                        2),
                    ForRussianFederation = ProductSalesInRussia(
                        SaleProductsByCategory(saleProducts, x.Id)),
                    PartnerSales = ProductSalesForPartner(
                        SaleProductsByCategory(saleProducts, x.Id)),
                    TurnOver = Math.Round(
                        IncomeBySales(
                            SaleProductsByCategory(saleProducts, x.Id)),
                        2),
                    TurnOverMoscow = Math.Round(
                        IncomeBySales(
                            SaleProductsByShop(
                                SaleProductsByCategory(saleProducts, x.Id), 1)),
                        2),
                    TurnOverPetersburg = Math.Round(
                        IncomeBySales(
                            SaleProductsByShop(
                                SaleProductsByCategory(saleProducts, x.Id), 2)),
                        2),
                    TurnOverRF = Math.Round(
                        IncomeBySales(
                            SaleProductsInRussia(
                                SaleProductsByCategory(saleProducts, x.Id))),
                        2),
                    TurnOverPartner = Math.Round(
                        IncomeBySales(
                            SaleProductsForPartner(
                                SaleProductsByCategory(saleProducts, x.Id))),
                        2),
                    MarginMoscow = Math.Round(
                        IncomeBySales(
                            SaleProductsByShop(
                                SaleProductsByCategory(saleProducts, x.Id), 1))
                        - PurchaseProductPrice(
                            ProductInformationsByShop(
                                ProductInformationsByCategory(productInformations, x.Id), 1)),
                        2),
                    MarginPetersburg = Math.Round(
                        IncomeBySales(
                            SaleProductsByShop(
                                SaleProductsByCategory(saleProducts, x.Id), 2))
                        - PurchaseProductPrice(
                            ProductInformationsByShop(
                                ProductInformationsByCategory(productInformations, x.Id), 2)),
                        2),
                    MarginPartner = Math.Round(
                        IncomeBySales(
                            SaleProductsForPartner(
                                SaleProductsByCategory(saleProducts, x.Id)))
                        - PurchaseProductPrice(
                            ProductInformationsForPartner(
                                ProductInformationsByCategory(productInformations, x.Id))),
                        2),
                    MarginRF = Math.Round(
                        IncomeBySales(
                            SaleProductsInRussia(
                                SaleProductsByCategory(saleProducts, x.Id)))
                        - PurchaseProductPrice(
                            ProductInformationsInRussia(
                                ProductInformationsByCategory(productInformations, x.Id))),
                        2)
                });
        }

        private IQueryable<SaleProduct> SaleProductsByCategory(IQueryable<SaleProduct> saleProducts,
            int categoryId)
        {
            return saleProducts.Where(x => x.Product.CategoryId == categoryId);
        }

        private IQueryable<SaleProduct> SaleProductsByShop(IQueryable<SaleProduct> saleProducts,
            int shopId)
        {
            return saleProducts.Where(x => x.Sale.ShopId == shopId
                                           && x.Sale.PartnerId == null
                                           && x.Sale.ForRussian == false);
        }

        private IQueryable<SaleProduct> SaleProductsForPartner(IQueryable<SaleProduct> saleProducts)
        {
            return saleProducts.Where(x => x.Sale.PartnerId != null);
        }

        private IQueryable<SaleProduct> SaleProductsInRussia(IQueryable<SaleProduct> saleProducts)
        {
            return saleProducts.Where(x => x.Sale.ForRussian
                                           && x.Sale.PartnerId == null);
        }

        private IQueryable<ProductInformation> ProductInformationsByCategory(
            IQueryable<ProductInformation> productInformations,
            int categoryId)
        {
            return productInformations.Where(x => x.Product.CategoryId == categoryId);
        }

        private IQueryable<ProductInformation> ProductInformationsByShop(
            IQueryable<ProductInformation> productInformations,
            int shopId)
        {
            return productInformations.Where(x => x.Sale.ShopId == shopId
                                                  && x.Sale.PartnerId == null
                                                  && x.Sale.ForRussian == false);
        }

        private IQueryable<ProductInformation> ProductInformationsForPartner(
            IQueryable<ProductInformation> productInformations)
        {
            return productInformations.Where(x => x.Sale.PartnerId != null);
        }

        private IQueryable<ProductInformation> ProductInformationsInRussia(
            IQueryable<ProductInformation> productInformations)
        {
            return productInformations.Where(x => x.Sale.ForRussian
                                                  && x.Sale.PartnerId == null);
        }

        private int ProductSalesByShop(IQueryable<SaleProduct> saleProducts, int shopId)
        {
            return SaleProductsByShop(saleProducts, shopId)
                .Sum(x => x.Amount);
        }

        private int ProductSalesInRussia(IQueryable<SaleProduct> saleProducts)
        {
            return SaleProductsInRussia(saleProducts)
                .Sum(x => x.Amount);
        }

        private int ProductSalesForPartner(IQueryable<SaleProduct> saleProducts)
        {
            return SaleProductsForPartner(saleProducts)
                .Sum(x => x.Amount);
        }

        private decimal IncomeBySales(IQueryable<SaleProduct> saleProducts)
        {
            return saleProducts.Sum(x => x.Cost * x.Amount - UnitProductDiscount(x.Sale));
        }

        private decimal UnitProductDiscount(Sale sale)
        {
            return sale.Discount / sale.SalesProducts.Count;
        }

        private decimal PurchaseProductPrice(IQueryable<ProductInformation> productInformations)
        {
            return productInformations.Sum(x => x.FinalCost);
        }
    }
}