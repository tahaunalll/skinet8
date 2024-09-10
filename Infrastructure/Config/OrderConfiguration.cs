using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne (x=>x.ShippingAddress, o=>o.WithOwner());
            builder.OwnsOne (x=>x.PaymentSummary, o=>o.WithOwner());
            //Enum db ye eklenirken string olarak eklensin diye db de kayıt --> Pending olarak eklenecek
            builder.Property (x=>x.Status).HasConversion(
                o=>o.ToString(),
                //dbden veri çekildiğinde sisteme Enum olarak gelsin diye --> OrderStatus.Shipped olarak modele atanır
                o=>(OrderStatus)Enum.Parse(typeof(OrderStatus), o)
            );
            builder.Property(x=>x.Subtotal).HasColumnType("decimal(18,2)");
            builder.HasMany(x=>x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            builder.Property(x=>x.OrderDate).HasConversion(
                d=>d.ToUniversalTime(),
                d=>DateTime.SpecifyKind(d,DateTimeKind.Utc)
            );

            
        }
    }
}