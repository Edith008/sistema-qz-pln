using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ExtranetQz.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace ExtranetQz
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            var dataProtectionProviderType = typeof(DataProtectorTokenProvider<IdentityUser>);

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddTokenProvider(TokenOptions.DefaultProvider, dataProtectionProviderType);
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();


            services.ConfigureApplicationCookie(options => {
                //Obtiene o establece un valor que especifica si un script del
                //lado del cliente puede acceder a una cookie.
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.LoginPath = "/Home/Index";
                options.AccessDeniedPath = "/Usuario/Account/AccessDenied";
            });
            services.AddSession(options => {
                options.Cookie.Name = ".PDHN.Session";
                options.IdleTimeout = TimeSpan.FromHours(12);
            });
            services.AddAuthorization(options => {
                options.AddPolicy("Authorization",policy => policy.RequireRole("Admin","User"));
            });

            services.Configure<IdentityOptions>(options => {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.MaxFailedAccessAttempts = 2;
                options.Lockout.AllowedForNewUsers = true;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseExceptionHandler(options => {
                //    options.Run(async context => {
                //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //        context.Response.ContentType = "text/html";
                //        var ex = context.Features.Get<IExceptionHandlerFeature>();
                //        if (ex != null)
                //        {
                //            var error = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace }";
                //            await context.Response.WriteAsync(error).ConfigureAwait(false);
                //        }
                //    });
                //});
                //app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseStatusCodePages("text/plain", "Pagina de codigos de estado, codigo de estado: {0}");
            //app.UseStatusCodePages(async context => {
            //    await context.HttpContext.Response.WriteAsync(
            //        "Pagina de codigos de estado, codigo de estado:" +
            //        context.HttpContext.Response.StatusCode
            //        );
            //});
            //app.UseStatusCodePagesWithRedirects("/ExtranetQz/Metodo?code={0}");
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute("Usuario","Usuario", "{controller=Usuario}/{action=Usuario}/{id?}");

                endpoints.MapAreaControllerRoute("Reporte", "Reporte", "{controller=Reporte}/{action=Reporte}/{id?}");

                endpoints.MapAreaControllerRoute("Principal", "Principal", "{controller=Principal}/{action=Principal}/{id?}");

                endpoints.MapAreaControllerRoute("Perfil", "Perfil", "{controller=Perfil}/{action=Perfil}/{id?}");

                endpoints.MapAreaControllerRoute("Inventario", "Inventario", "{controller=Inventario}/{action=Inventario}/{id?}");

                endpoints.MapAreaControllerRoute("Calculadora", "Calculadora", "{controller=Calculadora}/{action=Calculadora}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
