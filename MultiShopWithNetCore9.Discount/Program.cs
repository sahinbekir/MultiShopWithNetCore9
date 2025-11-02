
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models; // Swagger için

using Microsoft.OpenApi.Models;
using MultiShopWithNetCore9.Discount.Context;
using MultiShopWithNetCore9.Discount.Services;

var builder = WebApplication.CreateBuilder(args);


// JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001"; // AuthServer
        options.RequireHttpsMetadata = false;          // DEV
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "multishop.api");
    });
});




builder.Services.AddTransient<DapperContext>();
builder.Services.AddTransient<IDiscountService, DiscountService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// 🔹 Swagger ekle
builder.Services.AddEndpointsApiExplorer();
// Swagger + OAuth2 (client credentials)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Discount API", Version = "v1" });

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
                    { "multishop.api", "MultiShop API" }
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
            new[] { "multishop.api" }
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
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Discount API v1");

        // OAuth2 client ayarları (AuthServer/Config.cs ile uyumlu)
        opt.OAuthClientId("swagger");
        opt.OAuthClientSecret("secret");
        opt.OAuthScopes("multishop.api");

        // Client Credentials’ta PKCE gerekmez; ek bir ayar şart değil
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
