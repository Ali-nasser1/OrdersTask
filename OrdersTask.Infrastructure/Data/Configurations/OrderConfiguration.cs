using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersTask.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.Property(o => o.CustomerName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(o => o.Product)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(o => o.Amount)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(o => o.CreatedAt)
                   .IsRequired();

            builder.HasIndex(o => o.CreatedAt);
        }
    }
}
