using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brook.Totp;
using Brook.Totp.WebLogin2FA.Cache;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Brook.Totp.WebLogin2FA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMemoryCache();

            services.AddSingleton<ICacheManage, CacheManage>();

            services.AddBrookTotp();

            //配置authorrize
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                  {
                      options.EventsType = typeof(CustomCookieAuthenticationEvents);
                      options.AccessDeniedPath = "/Account";
                      options.LoginPath = "/Account";
                  });

            services.AddScoped<CustomCookieAuthenticationEvents>();

            RegisterServices(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<BrookTotpDBContext>(options => options.UseInMemoryDatabase(databaseName: "BrookTotpDB"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ServiceContext.Instance = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });


            // Must go before UseMvc
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void RegisterServices(IServiceCollection services)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.FullName.StartsWith("Brook.Totp"))
                {
                    continue;
                }

                List<Type> ts = assembly.GetTypes().Where(a=>a.Name.EndsWith("Service")).ToList();

                var result = new Dictionary<Type, Type[]>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    var interfaceType = item.GetInterfaces();
                    foreach (var type in interfaceType)
                    {
                        services.AddScoped(type, item);
                    }
                    result.Add(item, interfaceType);
                }
            }
        }
    }


}

