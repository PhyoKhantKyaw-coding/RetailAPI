using MODEL.ApplicationConfig;
using REPOSITORY.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REPOSITORY.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ISaleDetailsRepository SaleDetail { get; }
    IProductRepository Products { get; }
    ISaleRepository Sales { get; }
    IRoleRepository Roles { get; }
    IUserRepository Users { get; }
    AppSettings AppSettings { get; }
    Task<int> SaveChangesAsync();
}
