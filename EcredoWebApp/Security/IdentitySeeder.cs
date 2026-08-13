using EcredoWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace EcredoWebApp.Security;

public static class IdentitySeeder
{
    public const string AdminRole = "Admin";
    public const string CustomerRole = "Customer";


    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var roleManager =
            services.GetRequiredService<
                RoleManager<IdentityRole<Guid>>>();

        var userManager =
            services.GetRequiredService<
                UserManager<ApplicationUser>>();


        // =====================================================
        // CREATE ROLES
        // =====================================================

        if (!await roleManager.RoleExistsAsync(AdminRole))
        {
            var result =
                await roleManager.CreateAsync(
                    new IdentityRole<Guid>(AdminRole));

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    "Unable to create the Admin role.");
            }
        }


        if (!await roleManager.RoleExistsAsync(CustomerRole))
        {
            var result =
                await roleManager.CreateAsync(
                    new IdentityRole<Guid>(CustomerRole));

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    "Unable to create the Customer role.");
            }
        }


        // =====================================================
        // ADMIN CREDENTIALS
        // =====================================================

        var adminEmail =
            configuration["IdentitySeed:AdminEmail"]
            ?? "admin@ecredo.com";

        var adminPassword =
            configuration["IdentitySeed:AdminPassword"]
            ?? "Admin@12345";


        // =====================================================
        // FIND OR CREATE ADMIN
        // =====================================================

        var admin =
            await userManager.FindByEmailAsync(
                adminEmail);


        if (admin == null)
        {
            admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),

                UserName = adminEmail,
                Email = adminEmail,

                EmailConfirmed = true,

                FirstName = "System",
                LastName = "Administrator",

                IsActive = true,

                CreatedAt = DateTime.UtcNow
            };


            var createResult =
                await userManager.CreateAsync(
                    admin,
                    adminPassword);


            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    createResult.Errors.Select(
                        e => e.Description));

                throw new InvalidOperationException(
                    $"Unable to create the initial administrator: {errors}");
            }
        }


        // =====================================================
        // ENSURE ADMIN IS ACTIVE
        // =====================================================

        if (!admin.IsActive)
        {
            admin.IsActive = true;

            var updateResult =
                await userManager.UpdateAsync(admin);

            if (!updateResult.Succeeded)
            {
                throw new InvalidOperationException(
                    "Unable to activate the administrator account.");
            }
        }


        // =====================================================
        // ENSURE ADMIN HAS ADMIN ROLE
        // =====================================================

        if (!await userManager.IsInRoleAsync(
                admin,
                AdminRole))
        {
            var roleResult =
                await userManager.AddToRoleAsync(
                    admin,
                    AdminRole);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    roleResult.Errors.Select(
                        e => e.Description));

                throw new InvalidOperationException(
                    $"Unable to assign the Admin role: {errors}");
            }
        }
    }
}