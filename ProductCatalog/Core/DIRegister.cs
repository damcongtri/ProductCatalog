using ProductCatalog.Service.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Web;
using ProductCatalog.Service.BusinessLogic;
using ProductCatalog.Repository.Common.UnitOfWorkBase;
using ProductCatalog.Repository.Common.DbContext;
using ProductCatalog.Repository.Interfaces;
using ProductCatalog.Repository;
using System;


namespace ProductCatalog.Core
{
    public static class DIRegister
    {
        public static void RegisterDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DatabaseContext>(options => options
            .UseSqlServer(builder.Configuration["ProCateConnectionString"])
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            builder.Services.AddScoped<IDbContext, DatabaseContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ICategoryService,CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
        }
    }
}
