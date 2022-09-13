using CarDealerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Contexts
{
    public class DealerDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public DealerDbContext(DbContextOptions option, IConfiguration config) : base(option)
        {
            this._config = config;
        }
        
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Address> Adresses {get;set;}
        public DbSet<User> Users {get;set;}
        public DbSet<Role> Roles {get;set;}
        public DbSet<LoggerEntity> Loggers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dealer>()
                .Property(d => d.DealerName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Car>()
                .Property(c => c.NameMark)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(100);
            
            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(e => e.NameRole)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //doesn't work with integration test and InMemory / How to deal with??
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DealersCar"), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            //optionsBuilder.UseSqlServer(_config.GetConnectionString("DealersCar"));
        }
    }
}
