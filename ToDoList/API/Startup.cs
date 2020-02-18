using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Service;
using API.Service.Interface;
using Data.ConnectionString;
using Data.Context;
using Data.Repository;
using Data.Repository.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Data.Repository.Data;

namespace API
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
            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("Storage"));
            }, ServiceLifetime.Transient);

            var connectionString = new ConnectionStrings(Configuration.GetConnectionString("Storage"));
            services.AddSingleton(connectionString);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IToDoListRepository, ToDoListRepository>();
            services.AddScoped<ISuppRepository, SuppRepository>();
            services.AddScoped<IItemmRepository, ItemmRepository>();
            services.AddScoped<DepartmentRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IToDoListService, ToDoListService>();
            services.AddScoped<ISuppService, SuppService>();
            services.AddScoped<IItemmService, ItemmService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
