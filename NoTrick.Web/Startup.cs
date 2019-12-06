using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NoTrick.Web.Services;

namespace NoTrick.Web {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages(options => {
                options.Conventions.AuthorizeFolder("/Admin", "IsAdmin");
                options.Conventions.AuthorizeFolder("/Supplier", "IsSupplier");
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddAuthorization(options => {
                options.AddPolicy("IsAdmin",
                    policy => { policy.Requirements.Add(new RolesAuthorizationRequirement(new[] {"Admin"})); });
                options.AddPolicy("IsSupplier",
                    policy => { policy.Requirements.Add(new RolesAuthorizationRequirement(new[] {"Supplier"})); });
            });
            
            services.AddHealthChecks();
            services.AddNoTricksDataServices(Configuration.GetConnectionString("NoTricks"));
            services.AddSingleton<SignInService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
