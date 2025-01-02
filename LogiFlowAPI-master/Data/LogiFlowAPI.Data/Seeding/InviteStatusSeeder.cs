namespace LogiFlowAPI.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LogiFlowAPI.Data.Models;
    using LogiFlowAPI.Data.Models.Enums;

    internal class InviteStatusSeeder : ISeeder
    {
        public async Task SeedAsync(LogiFlowDbContext dbContext, IServiceProvider serviceProvider)
        {
            IEnumerable<string> statuses = Enum.GetNames(typeof(InviteStatusOptions));

            foreach (var status in statuses)
            {
                await dbContext.InviteStatuses.AddAsync(new InviteStatus { Status = status });
            }
        }
    }
}
