using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Services.Abstract;
using Base.Services.Concrete;
using Data;
using Data.ModernServices.Abstract;
using Data.ModernServices.Concrete;
using Data.Services.Abstract;
using Data.Services.Abstract.Filtration;
using Data.Services.Concrete;
using Data.Services.Concrete.Filtration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebUI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            string connection =
                "Data Source=31.31.196.211;Initial Catalog=u0530276_bvdshop;User ID=u0530276_bvdshop_admin;Password=1234";
                //"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\Documents\\bvd7.mdf;Integrated Security=True;Connect Timeout=30";

                services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                        options => { options.LoginPath = new PathString("/Account/Login"); });
                
                services.AddDbContext<ShopContext>(options =>
                    options.UseSqlServer(connection, b => b.MigrationsAssembly("WebUI")));

                services.AddTransient(typeof(IBaseObjectService<>), typeof(BaseObjectService<>));
                services.AddTransient<DbContext, ShopContext>();
                services.AddTransient<IProductService, ProductService>();
                services.AddTransient<ISaleService, SaleService>();
                services.AddTransient<IShopService, ShopService>();
                services.AddTransient<IInfoMoneyService, InfoMoneyService>();
                services.AddTransient<IMoneyOperationService, MoneyOperationService>();
                services.AddTransient<IFileService, FileService>();
                services.AddTransient<IFiltrationService, FiltrationService>();
                services.AddTransient<IInfoProductService, InfoProductService>();
                services.AddTransient<IDataCompareService, DataCompareService>();
                services.AddTransient<IProductOperationService, ProductOperationService>();
                services.AddTransient<ISalesAmountService, SalesAmountService>();
                services.AddTransient<ITurnOverService, TurnOverService>();
                services.AddTransient<IPrimeCostService, PrimeCostService>();
                services.AddTransient<IMarginService, MarginService>();
                services.AddTransient<ISaleStatisticService, SaleStatisticService>();
                services.AddTransient<IMoneyStatisticService, MoneyStatisticService>();
                services.AddTransient<ISaleInfoService, SaleInfoService>();
                services.AddTransient<IBookingProductInformationService, BookingProductInformationService>();
                services.AddTransient<ISalesByCategoryService, SalesByCategoryService>();
                
                services.AddCors();

                services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            
            app.UseAuthentication();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}