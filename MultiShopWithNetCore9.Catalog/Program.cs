using System.Reflection;
using Microsoft.Extensions.Options;
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
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
