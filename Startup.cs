using HelloWorld.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace HelloWorld
{
    public class Startup
    {

        public Startup()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json");
            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddEntityFrameworkSqlite().AddDbContext<HelloWorldDBContext>();

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<HelloWorldDBContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseWelcomePage();

            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            app.UseFileServer();

            //Identity 中间件
            app.UseAuthentication();

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(ConfigureRoute);


            //app.Run(async (context) =>
            //{
            //    //throw new System.Exception("异常");
            //    var msg = Configuration["message"];
            //    await context.Response.WriteAsync(msg);
            //});


        }


        private void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            // Home/Index
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }


    }
}
