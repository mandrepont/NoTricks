using System.Collections.Generic;
using System.Linq;
using IdentityServer4;
using IdentityServer4.Models;

namespace NoTricks.Data {
    public static class IdentityConfig {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> {
                new ApiScope("notricks.api", "No Trick API"),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client> {
                //NoTricks Angular client
                new Client {
                  ClientId  = "notricks.frontend",
                  ClientName = "NoTricks Frontend",
                  AllowedGrantTypes = GrantTypes.Code,
                  RequireClientSecret = false,
                  
                  RedirectUris = { "http://localhost:4200/login-callback", "http://localhost:4200" },
                  PostLogoutRedirectUris = { "http://localhost:4200" },
                  AllowedCorsOrigins = { "http://localhost:4200" },
                  
                  AllowedScopes = {
                      IdentityServerConstants.StandardScopes.OpenId,
                      IdentityServerConstants.StandardScopes.Profile,
                      IdentityServerConstants.StandardScopes.Email,
                      ApiScopes.Single().Name //Risky code will throw
                  }
                }
            };
    }
}