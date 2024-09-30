using Microsoft.EntityFrameworkCore;
using Nimap.Models;

namespace Nimap.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category) 
                .WithMany(c => c.Products) 
                .HasForeignKey(p => p.CategoryId); 
        }

    }
}
