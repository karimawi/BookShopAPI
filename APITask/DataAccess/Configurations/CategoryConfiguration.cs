using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using APITask.Models.Entities;

namespace APITask.DataAccess.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table configuration
            builder.ToTable("Categories", "MasterSchema");

            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.CatName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.CatOrder)
                .IsRequired();

            builder.Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Ignore CreatedDate from database mapping (computed property)
            builder.Ignore(c => c.CreatedDate);

            // Index
            builder.HasIndex(c => c.CatName)
                .HasDatabaseName("IX_Categories_CatName");

            // Navigation properties
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
