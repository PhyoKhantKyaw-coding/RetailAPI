using Microsoft.Extensions.DependencyInjection;
using MODEL.ApplicationConfig;
using MODEL;
using REPOSITORY.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using REPOSITORY.UnitOfWork;
using BAL.IServices;
using BAL.Services;


namespace BAL.Shared;

public class ServiceManager
{
    public static void SetServiceInfo(IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContextPool<DataContext>(options =>
        {
            options.UseSqlServer(appSettings.ConnectionString);
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}
