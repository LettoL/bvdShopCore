using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
using Microsoft.AspNetCore.HttpOverrides;
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
                "Data Source=31.31.196.211;Initial Catalog=u0530276_TestBVD;User ID=u0530276_bvdshop_admin;Password=Cfvbhyfchb20";
                //"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Admin\\Documents\\bvd7.mdf;Integrated Security=True;Connect Timeout=30";

                services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                        options => { options.LoginPath = new PathString("/Account/Login"); });
                
                services.AddDbContext<ShopContext>(options =>
                    options.UseSqlServer(connection, b => b.MigrationsAssembly("WebUI")));

                services.AddScoped(typeof(IBaseObjectService<>), typeof(BaseObjectService<>));
                services.AddScoped<DbContext, ShopContext>();
                services.AddScoped<IProductService, ProductService>();
                services.AddScoped<ISaleService, SaleService>();
                services.AddScoped<IShopService, ShopService>();
                services.AddScoped<IInfoMoneyService, InfoMoneyService>();
                services.AddScoped<IMoneyOperationService, MoneyOperationService>();
                services.AddScoped<IFileService, FileService>();
                services.AddScoped<IFiltrationService, FiltrationService>();
                services.AddScoped<IInfoProductService, InfoProductService>();
                services.AddScoped<IDataCompareService, DataCompareService>();
                services.AddScoped<IProductOperationService, ProductOperationService>();
                services.AddScoped<ISalesAmountService, SalesAmountService>();
                services.AddScoped<ITurnOverService, TurnOverService>();
                services.AddScoped<IPrimeCostService, PrimeCostService>();
                services.AddScoped<IMarginService, MarginService>();
                services.AddScoped<ISaleStatisticService, SaleStatisticService>();
                services.AddScoped<IMoneyStatisticService, MoneyStatisticService>();
                services.AddScoped<ISaleInfoService, SaleInfoService>();
                services.AddScoped<IBookingProductInformationService, BookingProductInformationService>();
                services.AddScoped<ISalesByCategoryService, SalesByCategoryService>();
                
                services.AddCors();

                services.Configure<RequestLocalizationOptions>(options =>
                {
                    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ru-RU");
                    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("ru-RU") };
                });
                
                services.AddMvc();
                
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.KnownProxies.Add(IPAddress.Parse("68.183.78.15"));
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                Console.WriteLine("---------------------------------------------------------------");
                foreach (var header in context.Request.Headers)
                {
                    Console.WriteLine(header.Key + ": " + header.Value);
                }

                await next.Invoke();
            });
            
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            
            app.UseAuthentication();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod());
            
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