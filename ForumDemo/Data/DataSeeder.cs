using ForumDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Data
{
	public class DataSeeder
	{
		private ApplicationDbContext _context;
		public DataSeeder(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task SeedSuperUser()
		{
			var store = new RoleStore<IdentityRole>(_context);
			var userStore = new UserStore<ApplicationUser>(_context);



			var user = new ApplicationUser
			{
				UserName = "forumAdmin@admin.com",
				NormalizedUserName = "forumadmin@admin.com",
				Email = "admin@test.com",
				EmailConfirmed = true,
				LockoutEnabled = false,
				SecurityStamp = Guid.NewGuid().ToString(),

			};

			var hasher = new PasswordHasher<ApplicationUser>();
			var hasedPassword = hasher.HashPassword(user, "admin");

			user.PasswordHash = hasedPassword;

			var isAdminRole = _context.Roles.Any(role => role.Name == "Admin");

			if (!isAdminRole)
			{
				await store.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "admin" });
			}

			var isSuperUser = _context.Users.Any(u => u.NormalizedUserName == user.UserName);

			if (!isSuperUser)
			{
				await userStore.CreateAsync(user);
				await userStore.AddToRoleAsync(user, "Admin");
			}

			await _context.SaveChangesAsync();
			//return Task.CompletedTask;
		}
	}
}


