using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KenTan.DataLayer.Models
{
    public class ProductDbConext : DbContext
    {
        public ProductDbConext(DbContextOptions<ProductDbConext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductCode);
                entity.Property(e => e.ProductCode).IsRequired().HasMaxLength(36);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.DeliveryPrice).IsRequired();
            });
            
            modelBuilder.Entity<ProductOption>(entity =>
            {
                entity.HasKey(e => e.ProductOptionId);
                entity.Property(e => e.ProductOptionId).IsRequired().HasMaxLength(36);
                entity.Property(e => e.ProductCode).IsRequired().HasMaxLength(36);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(100);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
