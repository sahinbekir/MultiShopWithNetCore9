using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models; // Swagger için

using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MultiShopWithNetCore9.Catalog.Services.CategoryServices;
using MultiShopWithNetCore9.Catalog.Services.ProductDetailServices;
using MultiShopWithNetCore9.Catalog.Services.ProductImageServices;
using MultiShopWithNetCore9.Catalog.Services.ProductServices;
using MultiShopWithNetCore9.Catalog.Settings;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MongoDB(
    databaseUrl: "mongodb://localhost:27017/MultiShopLogs",
    collectionName: "AppLogs")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);





builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001"; // IdentityServer (AuthServer)
        options.RequireHttpsMetadata = false;          // DEV için
        options.TokenValidationParameters.ValidateAudience = true;
        options.Audience = "ResourceCatalog";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion(ctx =>
        {
            var raw = ctx.User.FindAll("scope").Select(c => c.Value);
            var scopes = new HashSet<string>(
                raw.SelectMany(v => v.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            );
            return scopes.Contains("CatalogFullPermission");
        });
    });
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ApiScope", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//        policy.RequireClaim("scope", "CatalogFullPermission");
//    });
//});





builder.Host.UseSerilog();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();

// AddSingleton: Uygulama (application) boyunca tek bir örnek oluşturulur.
// AddScoped: Her HTTP isteği (request) başına tek bir örnek oluşturulur.
// AddTransient: Her talep edildiğinde yeni bir örnek oluşturulur.

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddScoped<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// 🔹 Swagger ekle
builder.Services.AddEndpointsApiExplorer();
// Swagger + OAuth2 (client credentials)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });

    var oauthScheme = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "CatalogFullPermission", "MultiShop API Catalog" }
                }
            }
        }
    };

    c.AddSecurityDefinition("oauth2", oauthScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new[] { "CatalogFullPermission" }
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1");

        // OAuth2 client ayarları (AuthServer/Config.cs ile uyumlu)
        opt.OAuthClientId("swagger");
        opt.OAuthClientSecret("secret");
        opt.OAuthScopes("CatalogFullPermission");

        // Client Credentials’ta PKCE gerekmez; ek bir ayar şart değil
    });
}

app.UseHttpsRedirection();

//Add Middleware
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();
app.MapControllers().RequireAuthorization("ApiScope");

app.Run();
