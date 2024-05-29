using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orders.Shared.Entities;

namespace Orders.Backend.Data
{
    public class DataContext:IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext>options):base(options) 
        { 

        }       
        public DbSet<Country>countries { get; set; }
        public DbSet<Category> categories{ get; set; }
        public DbSet<State> states { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<TemporalOrder> TemporalOrders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(country => country.Name).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(x=>x.Name).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<State>().HasIndex(state => state.Name).IsUnique();
            modelBuilder.Entity<City>().HasIndex(city =>city.Name).IsUnique();
            DisableCascadingDelete(modelBuilder);

        }

        private void DisableCascadingDelete(ModelBuilder modelBuilder)
        {
            var relationships=modelBuilder.Model.GetEntityTypes().SelectMany(e=>e.GetForeignKeys());
            foreach (var relationship in relationships)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
