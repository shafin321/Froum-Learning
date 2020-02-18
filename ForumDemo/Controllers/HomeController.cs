using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForumDemo.Models;
using ForumDemo.ViewModels;

namespace ForumDemo.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPost _post;
		public HomeController(IPost post)
		{
			_post = post;
		}
		public IActionResult Index()
		{
			
			var latest = _post.GetLatestPost(5);
			var posts = latest.Select(post => new PostListingModel
			{
				Id = post.Id,
				Title = post.Title,
				Author = post.User.UserName,
				AuthorId = post.User.Id,
				AuthorRating = post.User.Rating,
				DatePosted = post.Created.ToString(),
				RepliesCount = post.Replies.Count(),
				//Forum = GetFroumFormPost(post),
				ForumName = post.Forum.Title,
				ForumImageUrl = post.Forum.ImageUrl,
				ForumId=post.Forum.Id

			});

			var viewmodel = new HomeIndexModel
			{
				LatestPost=posts,
				SearchQuery=""

			};

			return View(viewmodel);

		}       	
				

		}
	}

