using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using MODEL.Entities;

namespace MODEL;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Product> Pros { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleDetails> SaleDetails { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }

}
