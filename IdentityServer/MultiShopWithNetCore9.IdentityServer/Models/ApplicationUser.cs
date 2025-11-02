using Microsoft.AspNetCore.Identity;

namespace MultiShopWithNetCore9.IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string Surname { get; set; }
}
