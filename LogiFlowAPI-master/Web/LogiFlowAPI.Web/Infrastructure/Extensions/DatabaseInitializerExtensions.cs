namespace LogiFlowAPI.Web.Infrastructure.Extensions
{
    using LogiFlowAPI.Data;
    using LogiFlowAPI.Data.Seeding;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class DatabaseInitializerExtensions
    {
        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;
            var dbContext = serviceProvider.GetRequiredService<LogiFlowDbContext>();

            // Rapid development phase only
            RecreateDatabase(dbContext);

            //dbContext.Database.Migrate();

            var initializeSeeder = new LogiFlowDbContextSeeder();
            initializeSeeder.Execute(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();

            return app;
        }

        private static void RecreateDatabase(DbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}
