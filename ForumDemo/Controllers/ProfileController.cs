﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ForumDemo.Models;
using ForumDemo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ForumDemo.Controllers
{
    public class ProfileController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IApplicationUser _userService;
		private readonly IUpload _uploadService;
		private readonly IConfiguration _configuration;
		public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService,IUpload uploadService, IConfiguration configuration)
		{
			_userManager = userManager;
			_userService = userService;
			_uploadService = uploadService;
			_configuration = configuration;

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
            return View(model);
        }
		[HttpPost]
		public async Task<IActionResult> UploadProfileImage(IFormFile file)
		{
			var userId = _userManager.GetUserId(User);
			var connectionString = _configuration.GetConnectionString("AzureStorageConnetion");
			var container = _uploadService.GetBlobContainer(connectionString);

			var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
			var filename = Path.Combine(parsedContentDisposition.FileName.Trim('"'));

			var blockBlob = container.GetBlockBlobReference(filename);

			await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
			await _userService.SetProfileImage(userId, blockBlob.Uri);

			return RedirectToAction("Detail", "Profile", new { id = userId });
		}

		public IActionResult Index()
		{
			var profile = _userService.GetALL().
				OrderByDescending(users => users.Rating).
				Select(user => new ProfileModel
				{
					Email=user.Email,
					UserName=user.UserName,
					ProfileImageUrl=user.ProfileImageurl,
					UserRating=user.Rating.ToString(),
					MemberSince=user.MemberSince,

				});
			var model = new ProfileListModel
			{
				Profiles=profile,
			};
			return View(model);
		}
	}
}