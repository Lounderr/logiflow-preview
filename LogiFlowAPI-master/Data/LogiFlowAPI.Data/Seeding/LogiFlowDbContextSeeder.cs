namespace LogiFlowAPI.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class LogiFlowDbContextSeeder
    {
        public async Task Execute(LogiFlowDbContext dbContext, IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(dbContext);

            ArgumentNullException.ThrowIfNull(serviceProvider);

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(LogiFlowDbContextSeeder));

            var seeders = new List<ISeeder>
            {
                new RolesSeeder(),
                new InviteStatusSeeder(),
                new DeliveryStatusSeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Seeder {Name} done.", seeder.GetType().Name);
            }
        }
    }
}
