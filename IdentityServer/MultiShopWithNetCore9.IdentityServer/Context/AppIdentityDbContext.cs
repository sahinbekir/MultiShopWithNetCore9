using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiShopWithNetCore9.IdentityServer.Models;

namespace MultiShopWithNetCore9.IdentityServer.Context;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options) { }
}
