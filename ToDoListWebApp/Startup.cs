
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
using Microsoft.AspNetCore.Mvc.Infrastructure;

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




            services.AddIdentity<ApplicationUser, IdentityRole>(opts => {
                opts.User.RequireUniqueEmail = true;
                //opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            /*
            services.AddMvcCore(config =>
              config.Filters.Add(new ToDoListExceptionFilter(_env.IsDevelopment())))
              .AddJsonFormatters(j =>
              {
                  j.ContractResolver = new DefaultContractResolver();
                  j.Formatting = Formatting.Indented;
              });
              */
            //services.AddAuthentication(); 
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<ITeamRepo, TeamRepo>();
            services.AddScoped<ISupervisorRepo, SupervisorRepo>();
            services.AddScoped<IToDoItemRepo, ToDoItemRepo>();

            services.AddMvc();

            
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

            ApplicationDbContext.CreateAdminAccount(app.ApplicationServices,
      Configuration).Wait();
            ApplicationDbContext.CreateAuditorAccount(app.ApplicationServices,
      Configuration).Wait();
        }
    }
}
