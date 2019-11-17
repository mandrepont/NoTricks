using System.Linq;
using System.Reflection;
using NoTricks.Data;
using NoTricks.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection {
    
    public static class Extensions {
        public static IServiceCollection AddNoTricksDataServices(this IServiceCollection services,
            string connectionString) {
            
            services.AddSingleton(new NoTricksConnectionString(connectionString));
            //Add repositories (Might be an easier way to do this via generic types, but then we would be too close to a generic ORM which we cannot have in this project)
            services.AddSingleton<IAccountRepo, AccountRepo>();
            
            return services;
        }
    }
}