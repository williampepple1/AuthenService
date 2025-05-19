using AuthenService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenService.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            ArgumentNullException.ThrowIfNull(context);
            await context.Database.EnsureCreatedAsync();
            await context.SaveChangesAsync();
            await SeedRoles(roleManager);
            await SeedUsers(userManager, roleManager);
        }

        // Method to seed roles
        private static async Task SeedRoles(RoleManager<Role> roleManager)
        {
            string[] roleNames = { "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new Role
                    {
                        // Optionally set additional properties if needed
                        NormalizedName = roleName.ToUpper(),
                        Name = roleName,
                        CreationTime= DateTime.Now,
                        Description= "Default role for users"
                    };

                    var result = await roleManager.CreateAsync(role);

                    // Optional: Add error logging if role creation fails
                    if (!result.Succeeded)
                    {
                        throw new DataException($"Failed to create role {roleName}");
                    }
                }
            }
        }

        private static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var users = new List<User>
         {
             new()
             {
                  Email="TestUser@Test.com",
                  EmailConfirmed=true,
                  CreatedAt=new DateTime(2025,1,1,8,0,0,0,DateTimeKind.Utc),
                  UserName="Testuser"
             }

         };

            foreach (var user in users)
            {
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result = await userManager.CreateAsync(user, "micr0s0ft_");

                    if (result.Succeeded && await roleManager.RoleExistsAsync("User"))
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }
                }
            }
        }

        
    }
}
