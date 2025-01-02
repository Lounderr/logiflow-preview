namespace LogiFlowAPI.Data.Configurations
{
    using LogiFlowAPI.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ShippedProductConfiguration : IEntityTypeConfiguration<ShippedProduct>
    {
        public void Configure(EntityTypeBuilder<ShippedProduct> builder)
        {
            builder
                .HasMany(x => x.Tags)
                .WithMany(x => x.ShippedProducts)
                .UsingEntity<ShippedProductTag>();
        }
    }
}
