using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace AuthServer;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("ResourceCatalog")
            {
                Scopes={"CatalogFullPermission"}
            },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

    // OpenID Connect standart kimlik kaynaklarý
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
        };

    // MultiShop API için tek bir scope tanýmlýyoruz
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("multishop.api", "MultiShop API"),
            new ApiScope("CatalogFullPermission", "Full Auth for Catalog"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "Catalogswagger",
                ClientName = "Catalog Swagger Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "multishop.api", "CatalogFullPermission" } // hangi API eriþimi gerekliyse
            },


            //new Client
            //{
            //    ClientId = "CatalogUser",
            //    ClientName = "Catalog Web",
            //    RequireClientSecret = false,
            //    AllowedGrantTypes = GrantTypes.ClientCredentials,
            //    RedirectUris = { "https://localhost:5173/signin-oidc" },
            //    PostLogoutRedirectUris = { "https://localhost:5173/signout-callback-oidc" },

            //    AllowedScopes =
            //    { "openid", "profile", "CatalogFullPermission", "offline_access", IdentityServerConstants.LocalApi.ScopeName,
            //    IdentityServerConstants.StandardScopes.Email,
            //    IdentityServerConstants.StandardScopes.OpenId,
            //    },
            //    AllowOfflineAccess = true,
            //    // Geliþtirme ortamýnda CORS'a takýlmamak için izinli origin'ler:
            //    AllowedCorsOrigins = { "https://localhost:5173" },
            //    AccessTokenLifetime = 600
            //},
            new Client
            {
                ClientId = "CatalogUser",
                ClientName = "Catalog Web (Machine to Machine)",
                RequireClientSecret = true,
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                AllowedScopes =
                {
                    //"multishop.api",
                    "CatalogFullPermission",
                    IdentityServerConstants.LocalApi.ScopeName,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },

                AccessTokenLifetime = 600 // 10 dakika (isteðe göre artýr)
            },


            // Swagger/Postman vb. için Client Credentials client
            new Client
            {
                ClientId = "swagger",
                ClientName = "Swagger Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "multishop.api" }
            },

            // Web uygulamasý / SPA için Code + PKCE client (gerekirse URL'leri sonra özelleþtiririz)
            new Client
            {
                ClientId = "webapp",
                ClientName = "Web App (PKCE)",
                RequireClientSecret = false,
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:5173/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5173/signout-callback-oidc" },
                AllowedScopes = { "openid", "profile", "multishop.api", "offline_access" },
                AllowOfflineAccess = true,
                // Geliþtirme ortamýnda CORS'a takýlmamak için izinli origin'ler:
                AllowedCorsOrigins = { "https://localhost:5173" }
            }
        };
}


/*using Duende.IdentityServer.Models;

namespace AuthServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },
        };
}
*/