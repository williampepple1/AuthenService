using AuthenService.Domain.Entities;
using AuthenService.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace AuthenService.API.Configurations
{
    public static class SeedRecords
    {
        public static async Task SeedRecordsToDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                var context = serviceProvider.GetRequiredService<AppDbContext>();
                await DbInitializer.Initialize(context, roleManager, userManager);
            }
            catch (Exception ex)
            {
                // Log the error
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }

        }

    }
}
