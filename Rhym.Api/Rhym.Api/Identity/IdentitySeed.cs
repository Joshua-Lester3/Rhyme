using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Rhym.Api.Data;
using Rhym.Api.Models;

namespace Rhym.Api.Identity;

public class IdentitySeed
{
	public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, IOptions<AdminAccountOptions> adminAccountOptions)
	{
		// Seed Roles
		await SeedRolesAsync(roleManager);

		// Seed Admin User
		AdminAccountOptions options = adminAccountOptions.Value;

		await SeedAdminUserAsync(userManager, options);
	}

	private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
	{
		// Seed Roles
		if (!await roleManager.RoleExistsAsync(Roles.Admin))
		{
			await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
		}
	}

	private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager, AdminAccountOptions options)
	{
		// Seed Admin User
		if (await userManager.FindByEmailAsync(options.Email) == null)
		{
			AppUser user = new AppUser
			{
				UserName = "Thors",
				Email = options.Email,
			};

			IdentityResult result = userManager.CreateAsync(user, options.Password).Result;

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, Roles.Admin);
			}
		}
	}
}
