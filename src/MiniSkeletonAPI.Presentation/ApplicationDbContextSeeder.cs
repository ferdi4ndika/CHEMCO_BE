using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniSkeletonAPI.Domain.Entities;
using MiniSkeletonAPI.Infrastructure.Data;
using MiniSkeletonAPI.Infrastructure.Identity;

public static class ApplicationDbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Pastikan database sudah dibuat
        await context.Database.MigrateAsync();

        // Cek apakah user dummy sudah ada
        var existingUser = await userManager.Users.FirstOrDefaultAsync(u => u.Nik == "CH001");
        if (existingUser != null) return;

        var user = new ApplicationUser
        {
            UserName = "CH001",
            NormalizedUserName = "CH001",
            Email = "ch001@example.com",
            NormalizedEmail = "CH001@EXAMPLE.COM",
            EmailConfirmed = true,
            PhoneNumber = "08123456789",
            PhoneNumberConfirmed = true,
            Nik = "CH001",
            Role = "admin" // sesuaikan
        };

        var password = "CH001";
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            Console.WriteLine("Dummy user CH001 berhasil dibuat.");
        }
        else
        {
            Console.WriteLine("Gagal membuat user:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- {error.Description}");
            }
        }
    }
}
