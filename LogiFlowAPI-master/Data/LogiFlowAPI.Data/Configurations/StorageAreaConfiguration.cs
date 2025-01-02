namespace LogiFlowAPI.Data.Configurations
{
    using LogiFlowAPI.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StorageAreaConfiguration : IEntityTypeConfiguration<StorageArea>
    {
        public void Configure(EntityTypeBuilder<StorageArea> builder)
        {
            builder
                .HasMany(x => x.Tags)
                .WithMany(x => x.StorageAreas)
                .UsingEntity<StorageAreaTag>();
        }
    }
}
