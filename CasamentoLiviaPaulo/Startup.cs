using CasamentoLiviaPaulo.Models;
using CasamentoLiviaPaulo.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasamentoLiviaPaulo
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.Add(new ServiceDescriptor(typeof(PresenteRepository), new PresenteRepository(Configuration.GetConnectionString("ProdConnection"))));
            services.Add(new ServiceDescriptor(typeof(ImagensRepository), new ImagensRepository(Configuration.GetConnectionString("ProdConnection"))));
            services.Add(new ServiceDescriptor(typeof(PresentearRepository), new PresentearRepository(Configuration.GetConnectionString("ProdConnection"))));
            services.Add(new ServiceDescriptor(typeof(MercadoPagoRepository), new MercadoPagoRepository(Configuration.GetConnectionString("ProdConnection"))));
            services.Add(new ServiceDescriptor(typeof(AcessosRepository), new AcessosRepository(Configuration.GetConnectionString("ProdConnection"))));
            //services.Add(new ServiceDescriptor(typeof(PresenteRepository), new PresenteRepository(Configuration.GetConnectionString("DevConnection"))));
            //services.Add(new ServiceDescriptor(typeof(ImagensRepository), new ImagensRepository(Configuration.GetConnectionString("DevConnection"))));
            //services.Add(new ServiceDescriptor(typeof(PresentearRepository), new PresentearRepository(Configuration.GetConnectionString("DevConnection"))));
            //services.Add(new ServiceDescriptor(typeof(MercadoPagoRepository), new MercadoPagoRepository(Configuration.GetConnectionString("DevConnection"))));
            //services.Add(new ServiceDescriptor(typeof(AcessosRepository), new AcessosRepository(Configuration.GetConnectionString("DevConnection"))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //MercadoPago.SDK.AccessToken = "TEST-4009994650089143-011318-5321cc2d4e5483554f03ca91b03d68a0-191500391"; //DEV
            MercadoPago.SDK.AccessToken = "APP_USR-4009994650089143-011318-246e8f46adca36ac9600cb9f57b25a1f-191500391"; //PROD
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
