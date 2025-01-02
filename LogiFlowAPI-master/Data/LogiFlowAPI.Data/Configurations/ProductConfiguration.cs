namespace LogiFlowAPI.Data.Configurations
{
    using LogiFlowAPI.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasMany(x => x.Tags)
                .WithMany(x => x.Products)
                .UsingEntity<ProductTag>();
        }
    }
}
