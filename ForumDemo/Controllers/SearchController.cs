using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDemo.Models;
using ForumDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ForumDemo.Controllers
{
    public class SearchController : Controller
    {
		private readonly IPost _postService;
		public SearchController(IPost postService)
		{
			_postService = postService;
		}
        public IActionResult Results(string searchQuery)
        {
			var posts = _postService.GetFilteredPosts(searchQuery);
			var   noResults = (!string.IsNullOrEmpty(searchQuery) && !posts.Any());
			var postListings = posts.Select(post => new PostListingModel
			{
				Id = post.Id,
				AuthorId = post.User.Id,
				Author = post.User.UserName,
				AuthorRating = post.User.Rating,
				Title = post.Title,
				DatePosted = post.Created.ToString(),
				RepliesCount = post.Replies.Count(),
				Forum = BuildForumListingModel(post)

			} );

			var model = new SearchResultModel 
			{
				Posts=postListings,
				SearchQuery=searchQuery,
				EmptySearchResult=noResults,


			};

            return View(model);
        }

		private ForumListingModel BuildForumListingModel(Post post)
		{
			var forum = post.Forum;

			return new ForumListingModel
			{
				Id =forum.Id,
				ImageUrl= forum.ImageUrl,
				Title=forum.Title,
				Description=forum.Description

				
			};
		
		}


		[HttpPost]
		public IActionResult Search(string searchQuery)
		{
			return RedirectToAction("Results", new { searchQuery });
		}
	}
}