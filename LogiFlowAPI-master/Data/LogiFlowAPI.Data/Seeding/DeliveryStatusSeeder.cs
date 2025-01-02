namespace LogiFlowAPI.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LogiFlowAPI.Data.Models.Enums;

    internal class DeliveryStatusSeeder : ISeeder
    {
        public async Task SeedAsync(LogiFlowDbContext dbContext, IServiceProvider serviceProvider)
        {
            IEnumerable<string> statuses = Enum.GetNames(typeof(DeliveryStatusOptions));
            foreach (var status in statuses)
            {
                await dbContext.DeliveryStatuses.AddAsync(new Models.DeliveryStatus { Status = status });
            }
        }
    }
}
