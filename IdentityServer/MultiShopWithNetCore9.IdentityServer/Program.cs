using System.Globalization;
using System.Text;
using Duende.IdentityServer.Licensing;
using MultiShopWithNetCore9.IdentityServer;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // HostingExtensions zinciri
    var app = builder
        .ConfigureLogging()
        .ConfigureServices();            // Build() burada dönüyor

    // Pipeline async (seed vb.)
    await app.ConfigurePipelineAsync();  // <-- Burasý await olmalý

    app.UseIdentityServer();     // önce IdentityServer
    app.UseAuthentication();     // sonra auth
    app.UseAuthorization();      // sonra authorization

    // (Ýstersen usage summary kýsmýný buraya taþýyabilirsin)
    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}



//using System.Globalization;
//using System.Text;
//using AuthServer;
//using Duende.IdentityServer.Licensing;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using MultiShopWithNetCore9.IdentityServer.Context;
//using MultiShopWithNetCore9.IdentityServer.Models;
//using Serilog;

//Log.Logger = new LoggerConfiguration()
//    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
//    .CreateBootstrapLogger();

//Log.Information("Starting up");

//try
//{
//    var builder = WebApplication.CreateBuilder(args);

//    // 1) EF Core DbContext
//    builder.Services.AddDbContext<AppIdentityDbContext>(options =>
//        options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

//    // 2) ASP.NET Core Identity
//    builder.Services
//        .AddIdentity<ApplicationUser, IdentityRole>(options =>
//        {
//            options.Password.RequiredLength = 6;
//            options.Password.RequireNonAlphanumeric = false;
//            options.Password.RequireUppercase = false;
//            options.Password.RequireLowercase = false;
//            options.Password.RequireDigit = false;
//            options.User.RequireUniqueEmail = false;
//        })
//        .AddEntityFrameworkStores<AppIdentityDbContext>()
//        .AddDefaultTokenProviders();

//    var isBuilder = builder.Services.AddIdentityServer(options =>
//    {
//        options.Events.RaiseErrorEvents = true;
//        options.Events.RaiseInformationEvents = true;
//        options.Events.RaiseFailureEvents = true;
//        options.Events.RaiseSuccessEvents = true;

//        options.KeyManagement.Enabled = false; // önceki gibi kalsýn
//        if (builder.Environment.IsDevelopment())
//        {
//            options.Diagnostics.ChunkSize = 1024 * 1024 * 10;
//        }
//    })
//    .AddAspNetIdentity<ApplicationUser>()     // eklendi
//    .AddLicenseSummary()
//    .AddDeveloperSigningCredential();          // dev için


//    var app = builder
//        .ConfigureLogging()
//        .ConfigureServices()
//        .ConfigurePipelineAsync();

//    if (app.Environment.IsDevelopment())
//    {
//        app.Lifetime.ApplicationStopping.Register(() =>
//        {
//            var usage = app.Services.GetRequiredService<LicenseUsageSummary>();
//            Console.Write(Summary(usage));
//        });
//    }



//    app.Run();
//}
//catch (Exception ex) when (ex is not HostAbortedException)
//{
//    Log.Fatal(ex, "Unhandled exception");
//}
//finally
//{
//    Log.Information("Shut down complete");
//    Log.CloseAndFlush();
//}

//static string Summary(LicenseUsageSummary usage)
//{
//    var sb = new StringBuilder();
//    sb.AppendLine("IdentityServer Usage Summary:");
//    sb.AppendLine(CultureInfo.InvariantCulture, $"  License: {usage.LicenseEdition}");
//    var features = usage.FeaturesUsed.Count > 0 ? string.Join(", ", usage.FeaturesUsed) : "None";
//    sb.AppendLine(CultureInfo.InvariantCulture, $"  Business and Enterprise Edition Features Used: {features}");
//    sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.ClientsUsed.Count} Client Id(s) Used");
//    sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.IssuersUsed.Count} Issuer(s) Used");

//    return sb.ToString();
//}

/*
>dotnet new install Duende.Templates
>dotnet new list duende
>cd \MultiShopWithNetCore9
>mkdir IdentityServer
>cd \IdentityServer
>dotnet new duende-is-inmem -n AuthServer
>dotnet sln add .\AuthServer\AuthServer.csproj
* set AuthServer/appsettings.json:
* set Config.cs
>dotnet add .\MultiShopWithNetCore9.Order.WebApi package Microsoft.AspNetCore.Authentication.JwtBearer
* set Program.cs (optional)
>dotnet run --project .\IdentityServer\AuthServer\AuthServer.csproj

dotnet run --project "C:\Users\sahin\OneDrive\Masaüstü\MultiShopWithNetCore9\IdentityServer\AuthServer\AuthServer.csproj"

* set HostingExtensions.cs
//https://localhost:5001/

*set ApplicationUser IdentityDb SeedUser
*/