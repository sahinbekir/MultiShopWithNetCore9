using System.Globalization;
using AuthServer;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MultiShopWithNetCore9.IdentityServer.Context;
using MultiShopWithNetCore9.IdentityServer.Models;
using Serilog;
using Serilog.Filters;

namespace MultiShopWithNetCore9.IdentityServer;

internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        // Write most logs to the console but diagnostic data to a file.
        // See https://docs.duendesoftware.com/identityserver/diagnostics/data
        builder.Host.UseSerilog((ctx, lc) =>
        {
            lc.WriteTo.Logger(consoleLogger =>
            {
                consoleLogger.WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    formatProvider: CultureInfo.InvariantCulture);
                if (builder.Environment.IsDevelopment())
                {
                    consoleLogger.Filter.ByExcluding(Matching.FromSource("Duende.IdentityServer.Diagnostics.Summary"));
                }
            });
            if (builder.Environment.IsDevelopment())
            {
                lc.WriteTo.Logger(fileLogger =>
                {
                    fileLogger
                        .WriteTo.File("./diagnostics/diagnostic.log", rollingInterval: RollingInterval.Day,
                            fileSizeLimitBytes: 1024 * 1024 * 10, // 10 MB
                            rollOnFileSizeLimit: true,
                            outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                            formatProvider: CultureInfo.InvariantCulture)
                        .Filter
                        .ByIncludingOnly(Matching.FromSource("Duende.IdentityServer.Diagnostics.Summary"));
                }).Enrich.FromLogContext().ReadFrom.Configuration(ctx.Configuration);
            }
        });
        return builder;
    }

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        // (A) DbContext
        builder.Services.AddDbContext<AppIdentityDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

        // (B) ASP.NET Core Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
        {
            opt.Password.RequiredLength = 6;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireDigit = false;
            opt.User.RequireUniqueEmail = false;
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddLocalApiAuthentication();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(IdentityServerConstants.LocalApi.PolicyName, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.AddAuthenticationSchemes(IdentityServerConstants.LocalApi.AuthenticationScheme);
            });


        // (D) API Controller’larý EKLE (404 ilacý)
        builder.Services.AddControllers();

        // Swagger UI (Order.WebApi) origin'ine izin ver
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("swagger", p =>
                p.WithOrigins
                    (
                    "https://localhost:7081", // Order.WebApi Swagger
                    "https://localhost:7043", // Order.WebApi Swagger
                    "https://localhost:7050", // Catalog.WebApi Swagger
                    "https://localhost:7251"  // Discount.WebApi Swagger
                    )
                 .AllowAnyHeader()
                 .AllowAnyMethod());
        });

        var isBuilder = builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            // Community lisansýný açýkça bildir (appsettings varsa onu kullan, yoksa 'community')
            //options.LicenseKey = builder.Configuration["Duende:LicenseKey"] ?? "community";

            // Otomatik anahtar yönetimini kapat (Business/Enterprise gerektirir)
            options.KeyManagement.Enabled = false;

            // Use a large chunk size for diagnostic logs in development where it will be redirected to a local file
            if (builder.Environment.IsDevelopment())
            {
                options.Diagnostics.ChunkSize = 1024 * 1024 * 10; // 10 MB
            }
        })
            .AddInMemoryApiResources(Config.ApiResources)
        //.AddTestUsers(TestUsers.Users)
        .AddAspNetIdentity<ApplicationUser>()
        .AddLicenseSummary()

        // KeyManagement'ý kapatýnca bir imzalama anahtarý vermemiz gerekir
        .AddDeveloperSigningCredential(); // DEV için yeterli

        // in-memory, code config
        isBuilder.AddInMemoryIdentityResources(Config.IdentityResources);
        isBuilder.AddInMemoryApiScopes(Config.ApiScopes);
        isBuilder.AddInMemoryClients(Config.Clients);


        // if you want to use server-side sessions: https://blog.duendesoftware.com/posts/20220406_session_management/
        // then enable it
        //isBuilder.AddServerSideSessions();
        //
        // and put some authorization on the admin/management pages
        //builder.Services.AddAuthorization(options =>
        //       options.AddPolicy("admin",
        //           policy => policy.RequireClaim("sub", "1"))
        //   );
        //builder.Services.Configure<RazorPagesOptions>(options =>
        //    options.Conventions.AuthorizeFolder("/ServerSideSessions", "admin"));


        builder.Services.AddAuthentication()
            .AddOpenIdConnect("oidc", "Sign-in with demo.duendesoftware.com", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.SaveTokens = true;

                options.Authority = "https://demo.duendesoftware.com";
                options.ClientId = "interactive.confidential";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

        // (Ýsteðe baðlý) Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder.Build();
    }


    public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();
        app.UseRouting();

        // CORS
        app.UseCors("swagger");

        app.UseIdentityServer();
        app.UseAuthentication();   // <-- EKLE
        app.UseAuthorization();

        app.MapControllers();      // <-- EKLE (api/Register artýk var)

        app.MapRazorPages()
            .RequireAuthorization();

        app.MapGet("/ok", () => "ok");

        //await SeedData.EnsureSeedAsync(app);


        return app;
    }


    public static class SeedData
    {
        public static async Task EnsureSeedAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var user = await userMgr.FindByNameAsync("admin");
            if (user == null)
            {
                user = new ApplicationUser { UserName = "admin", Email = "admin@local" };
                var result = await userMgr.CreateAsync(user, "Aa.123456"); // DEV þifresi
                if (!result.Succeeded) throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
