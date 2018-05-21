
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoListWebApp.Data;
using ToDoListWebApp.Models;
using ToDoListWebApp.Services;
using ToDoListWebApp.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ToDoListWebApp.Repos.Interfaces;
using ToDoListWebApp.Repos;

namespace ToDoListWebApp
{
    public class Startup
    {
        private IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _env = environment; 
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvcCore(config =>
              config.Filters.Add(new ToDoListExceptionFilter(_env.IsDevelopment())))
              .AddJsonFormatters(j =>
              {
                  j.ContractResolver = new DefaultContractResolver();
                  j.Formatting = Formatting.Indented;
              });


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            services.AddScoped<ITeamRepo, TeamRepo>();
            services.AddScoped<ISupervisorRepo, SupervisorRepo>();
            services.AddScoped<IToDoItemRepo, ToDoItemRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
