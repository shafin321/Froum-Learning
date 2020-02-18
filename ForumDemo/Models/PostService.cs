using ForumDemo.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Models
{
	public class PostService : IPost
	{
		private readonly ApplicationDbContext _context;
	
		public PostService(ApplicationDbContext context)
		{
			_context = context;
			
		}
		public async Task Add(Post post)
		{
			_context.Add(post);
			await _context.SaveChangesAsync();
			
			
		}

		public async Task AddReply(PostReply reply)
		{
			_context.PostReplies.Add(reply);
			await  _context.SaveChangesAsync();
		}

		public Post Create(Post post)
		{
			_context.Add(post);
			_context.SaveChanges();
			return post;
		}

		public Post Delete(int id)
		{
			throw new NotImplementedException();
		}

		public Post EditContent(int id, string newContent)
		{
			throw new NotImplementedException();
		}

		public Task EditPostContent(int id, string newContent)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Post> GetAll()
		{
			return _context.Posts.Include(post => post.Forum)
			   .Include(post => post.User)
			   .Include(post => post.Replies)
			   .ThenInclude(reply => reply.User);
		}

		public Post GetById(int id)
		{
			return _context.Posts.Where(post => post.Id == id)
			   .Include(post => post.Forum)
			   .Include(post => post.User)
			   .Include(post => post.Replies)
			   .ThenInclude(reply => reply.User)
			   .FirstOrDefault();
		}

		public IEnumerable<Post> GetFilteredPosts(int id, string searchQuery)
		{

			var forum = _context.Forums.Find(id);
			return string.IsNullOrEmpty(searchQuery)
				? _context.Posts
				: _context.Posts.Where(post
					=> post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery));
		}

		public IEnumerable<Post> GetFilteredPosts(string searchQuery)
		{
		return	_context.Posts.Include(post => post.Forum)
			   .Include(post => post.User)
			   .Include(post => post.Replies)
			   .ThenInclude(reply => reply.User).Where(post
					=> post.Title.Contains(searchQuery) || post.Content.Contains(searchQuery));
		}

		public IEnumerable<Post> GetLatestPost(int Count)
		{
			return GetAll().OrderByDescending(p => p.Created).Take(Count);
		}

		

		public IEnumerable<Post> GetPostByForum(int id)
		{
			 return _context.Forums.Where(f => f.Id == id).
				FirstOrDefault().Posts;
		}

		public Post ReplyPost(PostReply reply)
		{
			throw new NotImplementedException();
		}
	}
}
