namespace LogiFlowAPI.Data.Configurations
{
    using System;

    using LogiFlowAPI.Data.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class InviteConfiguration : IEntityTypeConfiguration<Invite>
    {
        private static readonly int InviteExpiryInHours = 48;

        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder.HasQueryFilter(e => DateTime.UtcNow < e.CreatedOn.AddHours(InviteExpiryInHours));
        }
    }
}
