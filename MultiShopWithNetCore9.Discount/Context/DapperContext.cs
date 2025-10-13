using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MultiShopWithNetCore9.Discount.Entities;

namespace MultiShopWithNetCore9.Discount.Context;

//Why Dapper (Ado.Net - Dapper - EfCore)
public class DapperContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=ERIQ\\SQLEXPRESS; initial Catalog=MultiShopDiscountDb; integrated Security=true; TrustServerCertificate=True");
    }

    public DbSet<Coupon> Coupones { get; set; }
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
