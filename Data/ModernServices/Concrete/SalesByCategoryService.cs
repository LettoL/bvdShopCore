using System;
using System.Collections.Generic;
using System.Linq;
using Data.Entities;
using Data.ModernServices.Abstract;
using Data.ViewModels;

namespace Data.ModernServices.Concrete
{
    public class SalesByCategoryService : ISalesByCategoryService
    {
        public ICollection<SaleByCategoryVM> SaleByCategory(ShopContext db,
            ICollection<SaleProduct> saleProducts,
            ICollection<ProductInformation> productInformations)
        {
            return db.Categories
                .Select(x => new SaleByCategoryVM()
                {
                    Category = x,
                    SalesByMoscow = ProductSalesByShop(
                        SaleProductsByCategory(saleProducts, x.Id), 1),
                    SalesByPetersburg = ProductSalesByShop(
                        SaleProductsByCategory(saleProducts, x.Id), 2),
                    SalesBySamara = ProductSalesByShop(
                        SaleProductsByCategory(saleProducts, x.Id), 27),
                    SalesByMoscowSever = ProductSalesByShop(
                        SaleProductsByCategory(saleProducts, x.Id), 29),
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
                    TurnOverSamara = Math.Round(
                        IncomeBySales(
                            SaleProductsByShop(
                                SaleProductsByCategory(saleProducts, x.Id),27)),
                        2),
                    TurnOverMoscowSever = Math.Round(
                        IncomeBySales(
                            SaleProductsByShop(
                                SaleProductsByCategory(saleProducts, x.Id),29)),
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
                    MarginSamara = Math.Round(
                        IncomeBySales(
                            SaleProductsByShop(
                                SaleProductsByCategory(saleProducts, x.Id), 27))
                        - PurchaseProductPrice(
                            ProductInformationsByShop(
                                ProductInformationsByCategory(productInformations, x.Id), 27)),
                        2),
                    MarginMoscowSever = Math.Round(
                        IncomeBySales(
                            SaleProductsByShop(
                                SaleProductsByCategory(saleProducts, x.Id), 29))
                        - PurchaseProductPrice(
                            ProductInformationsByShop(
                                ProductInformationsByCategory(productInformations, x.Id), 29)),
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
                }).ToList();
        }

        private static ICollection<SaleProduct> SaleProductsByCategory(ICollection<SaleProduct> saleProducts,
            int categoryId)
        {
            return saleProducts.Where(x => x.Product.CategoryId == categoryId).ToList();
        }

        private static ICollection<SaleProduct> SaleProductsByShop(ICollection<SaleProduct> saleProducts,
            int shopId)
        {
            return saleProducts.Where(x => x.Sale.ShopId == shopId
                                           && x.Sale.PartnerId == null
                                           && x.Sale.ForRussian == false).ToList();
        }

        private static ICollection<SaleProduct> SaleProductsForPartner(ICollection<SaleProduct> saleProducts)
        {
            return saleProducts.Where(x => x.Sale.PartnerId != null).ToList();
        }

        private static ICollection<SaleProduct> SaleProductsInRussia(ICollection<SaleProduct> saleProducts)
        {
            return saleProducts.Where(x => x.Sale.ForRussian
                                           && x.Sale.PartnerId == null).ToList();
        }

        private static ICollection<ProductInformation> ProductInformationsByCategory(
            ICollection<ProductInformation> productInformations,
            int categoryId)
        {
            return productInformations.Where(x => x.Product.CategoryId == categoryId).ToList();
        }

        private static ICollection<ProductInformation> ProductInformationsByShop(
            ICollection<ProductInformation> productInformations,
            int shopId)
        {
            return productInformations.Where(x => x.Sale.ShopId == shopId
                                                  && x.Sale.PartnerId == null
                                                  && x.Sale.ForRussian == false).ToList();
        }

        private static ICollection<ProductInformation> ProductInformationsForPartner(
            ICollection<ProductInformation> productInformations)
        {
            return productInformations.Where(x => x.Sale.PartnerId != null).ToList();
        }

        private static ICollection<ProductInformation> ProductInformationsInRussia(
            ICollection<ProductInformation> productInformations)
        {
            return productInformations.Where(x => x.Sale.ForRussian
                                                  && x.Sale.PartnerId == null).ToList();
        }

        private static int ProductSalesByShop(ICollection<SaleProduct> saleProducts, int shopId)
        {
            return SaleProductsByShop(saleProducts, shopId)
                .Sum(x => x.Amount);
        }

        private static int ProductSalesInRussia(ICollection<SaleProduct> saleProducts)
        {
            return SaleProductsInRussia(saleProducts)
                .Sum(x => x.Amount);
        }

        private static int ProductSalesForPartner(ICollection<SaleProduct> saleProducts)
        {
            return SaleProductsForPartner(saleProducts)
                .Sum(x => x.Amount);
        }

        private static decimal IncomeBySales(ICollection<SaleProduct> saleProducts)
        {
            return saleProducts.Sum(x => x.Cost * x.Amount - UnitProductDiscount(x.Sale));
        }

        private static decimal UnitProductDiscount(Sale sale)
        {
            return sale.Discount / sale.SalesProducts.Count;
        }

        private static decimal PurchaseProductPrice(ICollection<ProductInformation> productInformations)
        {
            return productInformations.Sum(x => x.FinalCost);
        }
    }
}