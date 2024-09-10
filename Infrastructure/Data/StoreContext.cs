using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // public class StoreContext(DbContextOptions options) : IdentityDbContext<AppUser>( options)
    // {
    //    public DbSet<Product> Products { get; set; }
    //        public DbSet<IdentityUserLogin<string>> IdentityUserLogins { get; set; }


    //     protected override void OnModelCreating(ModelBuilder modelBuilder)
    //     {
    //         //..Infrastructure/Config/ProductConfiguration içinde tanımlı
    //         modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    //         //modelbuilder.entity<> .. şeklinde gidilince burası tam bir mess olur
    //         modelBuilder.Entity<IdentityUserRole<string>>()
    //         .HasKey(ur => new { ur.UserId, ur.RoleId });
    //     }
    // }

     public class StoreContext : IdentityDbContext<AppUser>
    {
        public StoreContext(DbContextOptions<StoreContext> options) 
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Bu satırı ekleyin

            // ProductConfiguration içinde tanımlı
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);

            // IdentityUserRole için birincil anahtar tanımlaması
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

             modelBuilder.Entity<Address>()
            .HasKey(a => a.ID);
        }
    }
}
