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
    public class ReplyController : Controller
    {
		private readonly IPost _post;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IApplicationUser _user;
		public ReplyController(IPost post,UserManager<ApplicationUser> userManager,IApplicationUser user)
		{
			_post = post;
			_userManager = userManager;
			_user = user;
		}
        public async Task<IActionResult> Create(int id)
        {
			var post = _post.GetById(id);
			var user = await _userManager.FindByNameAsync(User.Identity.Name);

			var model = new PostReplyModel
			{
				Id=post.Id,
				PostContent=post.Content,
				PostTitle=post.Title,
				PostId=post.Id,
				AuthorId=user.Id,
				AuthorName=User.Identity.Name,
				AuthorImageUrl=user.ProfileImageurl,
				AuthorRating=user.Rating,
				IsAuthorAdmin=User.IsInRole("Admin"),

				ForumName=post.Forum.Title,
				ForumId=post.Forum.Id,
				ForumImageUrl=post.Forum.ImageUrl,


				Date=DateTime.Now,
										   
			};
            return View(model);

        }

		[HttpPost]
		public async Task<IActionResult>AddReply(PostReplyModel model)
		{
			var userId = _userManager.GetUserId(User);
			var user = await _userManager.FindByIdAsync(userId);

			var reply = BuildReply(model, user);

		await 	_post.AddReply(reply);
			await _user.IncreamentRating(userId, typeof(Post));

			return RedirectToAction("Index", "Post", new { id = model.PostId });
		}

		private PostReply BuildReply(PostReplyModel model, ApplicationUser user)
		{
			var post = _post.GetById(model.PostId);

			return new PostReply
			{
				Post = post,
				Content = model.ReplyContent,
				Created = DateTime.Now,
				User = user,
			};
	
		}
	}
}