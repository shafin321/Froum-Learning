using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ForumDemo.Models;
using ForumDemo.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ForumDemo.Controllers
{
    public class ForumController : Controller
    {
		private readonly IForum _forum;
		private readonly IPost _post;
		private readonly IApplicationUser _userService;
		private readonly IUpload _uploadService;
		private readonly IConfiguration _configuration;
		public ForumController(IForum forum, IPost post, IConfiguration configuration, IApplicationUser userService, IUpload uploadService)
		{
			_forum = forum;
			_post = post;
			_configuration = configuration;
			_userService = userService;
			_uploadService = uploadService;
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


		public IActionResult Create()
		{

			var model = new AddForumModel();

			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> AddForum(AddForumModel model)
		{
			var imageUri = "";

			if (model.ImageUpload != null)
			{
				var blockBlob = PostForumImage(model.ImageUpload);
				imageUri = blockBlob.Uri.AbsoluteUri;
			}

			else
			{
				imageUri = "/images/users/default.png";
			}

			var forum = new  Forum()
			{
				Title = model.Title,
				Description = model.Description,
				Created = DateTime.Now,
				ImageUrl = imageUri
			};

			 _forum.Create(forum);
			return RedirectToAction("Index", "Forum");
		}


		public CloudBlockBlob PostForumImage(IFormFile file)
		{
			var connectionString = _configuration.GetConnectionString("AzureStorageConnetion");
			var container = _uploadService.GetBlobContainer(connectionString);
			var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
			var filename = Path.Combine(parsedContentDisposition.FileName.ToString().Trim('"'));
			var blockBlob = container.GetBlockBlobReference(filename);
			blockBlob.UploadFromStreamAsync(file.OpenReadStream());

			return blockBlob;
		}
	}

	}
