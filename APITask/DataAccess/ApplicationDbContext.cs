using Microsoft.EntityFrameworkCore;
using APITask.Models.Entities;
using APITask.DataAccess.Configurations;

namespace APITask.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            // Global query filter for soft delete
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category 
                { 
                    Id = 1, 
                    CatName = "Fiction", 
                    CatOrder = 1, 
                    IsDeleted = false 
                },
                new Category 
                { 
                    Id = 2, 
                    CatName = "Science", 
                    CatOrder = 2, 
                    IsDeleted = false 
                },
                new Category 
                { 
                    Id = 3, 
                    CatName = "Technology", 
                    CatOrder = 3, 
                    IsDeleted = false 
                },
                new Category 
                { 
                    Id = 4, 
                    CatName = "Biography", 
                    CatOrder = 4, 
                    IsDeleted = false 
                },
                new Category 
                { 
                    Id = 5, 
                    CatName = "History", 
                    CatOrder = 5, 
                    IsDeleted = false 
                }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product 
                { 
                    Id = 1, 
                    Title = "The Great Adventure", 
                    Description = "An exciting fiction novel", 
                    Author = "John Smith", 
                    BookPrice = 29.99m, 
                    CategoryId = 1, 
                    IsDeleted = false 
                },
                new Product 
                { 
                    Id = 2, 
                    Title = "Physics Fundamentals", 
                    Description = "Basic principles of physics", 
                    Author = "Dr. Sarah Wilson", 
                    BookPrice = 45.50m, 
                    CategoryId = 2, 
                    IsDeleted = false 
                },
                new Product 
                { 
                    Id = 3, 
                    Title = "Modern Web Development", 
                    Description = "Complete guide to web technologies", 
                    Author = "Mike Johnson", 
                    BookPrice = 55.00m, 
                    CategoryId = 3, 
                    IsDeleted = false 
                },
                new Product 
                { 
                    Id = 4, 
                    Title = "Steve Jobs Biography", 
                    Description = "Life story of Apple founder", 
                    Author = "Walter Isaacson", 
                    BookPrice = 35.99m, 
                    CategoryId = 4, 
                    IsDeleted = false 
                },
                new Product 
                { 
                    Id = 5, 
                    Title = "World War II Chronicles", 
                    Description = "Comprehensive history of WWII", 
                    Author = "Robert Miller", 
                    BookPrice = 42.75m, 
                    CategoryId = 5, 
                    IsDeleted = false 
                }
            );
        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Category category && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    category.IsDeleted = true;
                }
                else if (entry.Entity is Product product && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    product.IsDeleted = true;
                }
            }
        }
    }
}
