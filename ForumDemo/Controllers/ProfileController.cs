using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDemo.Models;
using ForumDemo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumDemo.Controllers
{
    public class ProfileController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IApplicationUser _userService;
		public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService)
		{
			_userManager = userManager;
			_userService = userService;

		}
        public IActionResult Detail(string id)
        {
			var user = _userService.GetById(id);
			var userRole = _userManager.GetRolesAsync(user).Result;

			var model = new ProfileModel
			{
				UserId = user.Id,
				UserName = user.UserName,
				UserRating = user.Rating.ToString(),
				Email = user.Email,
				ProfileImageUrl = user.ProfileImageurl,
				MemberSince = user.MemberSince,
				IsAdmin = userRole.Contains("Admin"),

			};
            return View();
        }
    }
}