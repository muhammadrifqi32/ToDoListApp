using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class MyContext : IdentityDbContext
    {
        private readonly IServiceProvider _serviceProvider;
        public DbSet<User> Users { get; set; }
        public DbSet<ToDoList> ToDoList { get; set; }
        public DbSet<Supp> Supps { get; set; }
        public DbSet<Itemm> Itemms { get; set; }
        public DbSet<Department> Departments { get; set; }

        public MyContext() { }

        public MyContext(DbContextOptions<MyContext> options, IServiceProvider serviceProvider)
            : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("Storage");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
