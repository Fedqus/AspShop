using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection.Metadata;

namespace kurs.Models
{
    public class DatabaseContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<TagGroup> TagGroups { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.HasTrigger("trg_ProductChanged"));
            modelBuilder.Entity<ProductTag>()
                .ToTable(tb => tb.HasTrigger("trg_ProductTagChanged"));
            modelBuilder.Entity<Review>()
                .ToTable(tb => tb.HasTrigger("trg_ReviewChanged"));
            modelBuilder.Entity<OrderProduct>()
                .ToTable(tb => tb.HasTrigger("trg_UpdateProductAmount"));

            modelBuilder.Entity<Review>()
                .Property(x => x.Date)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}