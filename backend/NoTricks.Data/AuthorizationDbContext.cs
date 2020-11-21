using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoTricks.Data.Models;

namespace NoTricks.Data {
    public class AuthorizationDbContext : ApiAuthorizationDbContext<ApplicationUser> {

        public AuthorizationDbContext(DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}