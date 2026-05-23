using System.Collections.Generic;
using ERP_Stock_Movement.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP_Stock_Movement.Products.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Id).ValueGeneratedNever();
            });
        }
    }
}
