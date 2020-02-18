using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDemo.Models;
using ForumDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForumDemo.Controllers
{
    public class ForumController : Controller
    {
		private readonly IForum _forum;
		private readonly IPost _post;
		public ForumController(IForum forum, IPost post)
		{
			_forum = forum;
			_post = post;
		}
        public IActionResult Index()
        {
			var model = _forum.GetAll().
				Select(forum => new ForumListingModel
				{
					Id= forum.Id,
					Title=forum.Title,
					Description=forum.Description
				}
					);

            return View(model);
        }
		public IActionResult Topic(int id,string searchQuery)
		{
			var forum = _forum.GetById(id);
			var posts = _post.GetPostByForum(id);
		//	var posts = _post.GetFilteredPosts(id, searchQuery).ToList();
			//var noResults = (!string.IsNullOrEmpty(searchQuery) && !posts.Any());
			//var posts = new List<Post>();
			



			var poslistingModels = posts.Select(post => new PostListingModel
			{
				Id = post.Id,
				AuthorId = post.User.Id,
				AuthorRating=post.User.Rating,
				Title = post.Title,
				DatePosted = post.Created.ToString(),
				RepliesCount = post.Replies.Count(),
				Forum= BuildForumListing(post)


			});

			var model = new ForumTopicModel
			{
				//EmptySearchResults = noResults,
				Posts = poslistingModels,
				Forum = BuildForumListing(forum),
			   SearchQuery = searchQuery

			};
			


			return View(model);
		}

		private static ForumListingModel BuildForumListing(Forum forum)
		{
			return new ForumListingModel
			{
				Id = forum.Id,
				ImageUrl = forum.ImageUrl,
				Title= forum.Title,
				Description = forum.Description
			};
		}

		private static ForumListingModel BuildForumListing(Post post)
		{
			var forum = post.Forum;
			return BuildForumListing(forum);
		}

		[HttpPost]
		public IActionResult Search(int id, string searchQury)
		{
			return RedirectToAction("Topic", new { id, searchQury });
		}



	}

	}
