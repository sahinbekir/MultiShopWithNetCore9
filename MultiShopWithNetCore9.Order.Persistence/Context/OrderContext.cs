using Microsoft.EntityFrameworkCore;
using MultiShopWithNetCore9.Order.Domain.Entities;

namespace MultiShopWithNetCore9.Order.Persistence.Context;

public class OrderContext:DbContext 
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=ERIQ\\SQLEXPRESS; initial Catalog=MultiShopOrderDb; integrated Security=true; TrustServerCertificate=True");
    }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Ordering> Orderings { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
}
