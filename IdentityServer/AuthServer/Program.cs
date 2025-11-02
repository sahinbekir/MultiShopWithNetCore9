using System.Globalization;
using System.Text;
using Duende.IdentityServer.Licensing;
using AuthServer;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var app = builder
        .ConfigureLogging()
        .ConfigureServices()
        .ConfigurePipeline();

    if (app.Environment.IsDevelopment())
    {
        app.Lifetime.ApplicationStopping.Register(() =>
        {
            var usage = app.Services.GetRequiredService<LicenseUsageSummary>();
            Console.Write(Summary(usage));
        });
    }

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

static string Summary(LicenseUsageSummary usage)
{
    var sb = new StringBuilder();
    sb.AppendLine("IdentityServer Usage Summary:");
    sb.AppendLine(CultureInfo.InvariantCulture, $"  License: {usage.LicenseEdition}");
    var features = usage.FeaturesUsed.Count > 0 ? string.Join(", ", usage.FeaturesUsed) : "None";
    sb.AppendLine(CultureInfo.InvariantCulture, $"  Business and Enterprise Edition Features Used: {features}");
    sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.ClientsUsed.Count} Client Id(s) Used");
    sb.AppendLine(CultureInfo.InvariantCulture, $"  {usage.IssuersUsed.Count} Issuer(s) Used");

    return sb.ToString();
}

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
*/