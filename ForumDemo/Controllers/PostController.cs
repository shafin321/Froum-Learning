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
	public class PostController : Controller
	{
		private readonly IPost _post;
		private readonly IForum _forum;
		private static UserManager<ApplicationUser> _usermanager;


		public PostController(IPost post, IForum forum, UserManager<ApplicationUser>userManager)
		{
			_post = post;
			_forum = forum;
			_usermanager = userManager;

		}



		public IActionResult Index(int id)
		{
			var post = _post.GetById(id);
		    var replies = BuildPostRepliesps(post.Replies);

			var model = new PostIndexModel
			{

				Id = post.Id,
				Title = post.Title,
				AuthorId = post.User.Id,
				AuthorName = post.User.UserName,
				AuthorImageUrl = post.User.ProfileImageurl,
				AuthorRating = post.User.Rating,
			    IsAuthorAdmin = IsAuthorAdmin(post.User),
				Date = post.Created,
				PostContent = post.Content,
				Replies = replies,
				ForumId = post.Forum.Id,
				ForumName = post.Forum.Title


			};

			return View(model);
		}

		private bool IsAuthorAdmin(ApplicationUser user)
		{
			return _usermanager.GetRolesAsync(user).Result.Contains("Admin");
	
		}

		private IEnumerable<PostReplyModel> BuildPostRepliesps( IEnumerable<PostReply> replies)
			{
				return replies.Select(reply => new PostReplyModel
				{
					Id = reply.Id,
					AuthorName = reply.User.UserName,
					AuthorId = reply.User.Id,
					AuthorImageUrl = reply.User.ProfileImageurl,
					AuthorRating = reply.User.Rating,
					Date = reply.Created,
					ReplyContent = reply.Content,
					IsAuthorAdmin = IsAuthorAdmin(reply.User)
				});  

			}

		public IActionResult Create(int id)
		{
			var forum = _forum.GetById(id);

			var model = new NewPostModel
			{
				ForumName= forum.Title,
				ForumId=forum.Id,
				ForumImageUrl=forum.ImageUrl,
				AuthorName= User.Identity.Name,
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(NewPostModel model)
		{
			// userManager intract with user and data
			var userId = _usermanager.GetUserId(User);
			var user = await  _usermanager.FindByIdAsync(userId);

			var post = BuildPost(model, user);

			  _post.Create(post);

			return RedirectToAction("Index","Post" ,new {post.Id});

			
		}

		private Post BuildPost(NewPostModel model, ApplicationUser user)
		{
			var forum = _forum.GetById(model.ForumId);
			return new Post
			{
				Title = model.Title,
				Content = model.Content,
				Created = DateTime.Now,
				User = user,
				Forum=forum

			};
		}
	}

			
		
	}

		

	
