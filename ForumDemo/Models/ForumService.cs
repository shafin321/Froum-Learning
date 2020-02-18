using ForumDemo.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Models
{

	public class ForumService : IForum
	{
		private readonly ApplicationDbContext _context;
		public ForumService(ApplicationDbContext context)
		{
			_context = context;
		}
		public Forum Create(Forum forum)
		{
			throw new NotImplementedException();
		}

		public Forum Delete(int id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Forum> GetAll()
		{
			return _context.Forums.Include(f=>f.Posts);
		}

		public IEnumerable<ApplicationUser> GetAllUsers()
		{
			throw new NotImplementedException();
		}

		public Forum GetById(int id)
		{
			return _context.Forums
			   .Where(f => f.Id == id)
			   .Include(f => f.Posts)
			   .ThenInclude(f => f.User)
			   .Include(f => f.Posts)
			   .ThenInclude(f => f.Replies)
			   .ThenInclude(f => f.User)
			   .Include(f => f.Posts)
			   .ThenInclude(p => p.Forum)
			   .FirstOrDefault();
		}

		public Forum UpdateForumDescription(int forumId, string newDescription)
		{
			throw new NotImplementedException();
		}

		public Forum UpdateForumTitle(int forumId, string newTitle)
		{
			throw new NotImplementedException();
		}
	}
}
