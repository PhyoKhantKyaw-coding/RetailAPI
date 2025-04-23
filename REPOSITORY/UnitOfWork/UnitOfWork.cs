using Microsoft.Extensions.Options;
using MODEL.ApplicationConfig;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.Entities;
using REPOSITORY.Repositories.IRepositories;
using REPOSITORY.Repositories.Repositories;
using System.Reflection.Metadata;

namespace REPOSITORY.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private DataContext _dataContext;
    public UnitOfWork(DataContext dataContext, IOptions<AppSettings> appsettings)
    {
        _dataContext = dataContext;
        AppSettings = appsettings.Value;
        SaleDetail = new SaleDetailsRepository(_dataContext);
        Products = new ProductRepository(_dataContext);
        Sales = new SaleRepository(_dataContext);
        Roles = new RoleRepository(_dataContext);
        Users = new UserRepository(_dataContext);
    }

    public AppSettings AppSettings { get; private set; }
    public ISaleDetailsRepository SaleDetail { get; private set; }
    public IProductRepository Products { get; private set; }
    public ISaleRepository Sales { get; private set; }
    public IRoleRepository Roles { get; private set; }
    public IUserRepository Users { get; private set; }

    public void Dispose()
    {
        _dataContext.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dataContext.SaveChangesAsync();
    }
}
