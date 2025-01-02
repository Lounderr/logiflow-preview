namespace LogiFlowAPI.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(LogiFlowDbContext dbContext, IServiceProvider serviceProvider);
    }
}
