using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

using MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.AddressHandlers;
using MultiShopWithNetCore9.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;
using MultiShopWithNetCore9.Order.Application.Interfaces;
using MultiShopWithNetCore9.Order.Application.Services;
using MultiShopWithNetCore9.Order.Persistence.Context;
using MultiShopWithNetCore9.Order.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001"; // IdentityServer (AuthServer)
        options.RequireHttpsMetadata = false;          // DEV için
        options.TokenValidationParameters.ValidateAudience = false;
        //options.Audience = "OrderAPI";
        // options.TokenValidationParameters.ValidTypes = new[] { "at+jwt", "JWT" }; // (gerekirse)
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "multishop.api");
    });
});

builder.Services.AddDbContext<OrderContext>();

// Add services to the container.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddApplicationService(builder.Configuration);

#region
builder.Services.AddScoped<GetAddressQueryHandler>();
builder.Services.AddScoped<GetAddressByIdQueryHandler>();
builder.Services.AddScoped<CreateAddressCommandHandler>();
builder.Services.AddScoped<UpdateAddressCommandHandler>();
builder.Services.AddScoped<RemoveAddressCommandHandler>();

builder.Services.AddScoped<GetOrderDetailQueryHandler>();
builder.Services.AddScoped<GetOrderDetailByIdQueryHandler>();
builder.Services.AddScoped<CreateOrderDetailCommandHandler>();
builder.Services.AddScoped<UpdateOrderDetailCommandHandler>();
builder.Services.AddScoped<RemoveOrderDetailCommandHandler>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });

    // OAuth2 (Client Credentials) tanýmý
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
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
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
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API v1");

        // OAuth2 client ayarlarý (AuthServer/Config.cs ile uyumlu)
        opt.OAuthClientId("swagger");
        opt.OAuthClientSecret("secret");
        opt.OAuthScopes("multishop.api");

        // Client Credentials’ta PKCE gerekmez; ek bir ayar þart deðil
    });
}

app.UseHttpsRedirection();

//Add Middleware
app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();
app.MapControllers().RequireAuthorization("ApiScope");

app.Run();

/*
>docker images
>docker volume create portainer_data
>docker run -d -p 8000:8000 -p 9000:9000 --name=portainer --restart=always -v /var/run/docker.sock:/var/run/docker.sock -v portainer_data:/data portainer/portainer-ce
>docker rm -f portainer
>docker run -d -p 8001:8000 -p 9443:9443 -p 9000:9000 --name portainer --restart=always -v portainer_data:/data -v /var/run/docker.sock:/var/run/docker.sock portainer/portainer-ce:latest
 */
//https://localhost:9443/
//admin Aa.1234567890