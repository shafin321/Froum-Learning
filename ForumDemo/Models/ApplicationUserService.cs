using ForumDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Models
{
	public class ApplicationUserService : IApplicationUser
	{
		private readonly ApplicationDbContext _context;
		public ApplicationUserService(ApplicationDbContext context)
		{
			_context = context;

		}
		public IEnumerable<ApplicationUser> GetALL()
		{
			return _context.ApplicationUsers;
		}

		public ApplicationUser GetById(string id)
		{
			return _context.ApplicationUsers.
				FirstOrDefault(user => user.Id == id);
		}
		public async Task IncreamentRating(string userId, Type type)
		{
			var user = GetById(userId);
			user.Rating = GetIncrement(type, user.Rating);

			await _context.SaveChangesAsync();


		}

		private int GetIncrement(Type type, int rating)
		{
			var inc = 0;
			if (type == typeof(Post))
				inc = 1;
			if (type == typeof(PostReply))
				inc = 3;
			return rating + inc;
		}

		public async Task SetProfileImage(string id, Uri uri)
		{
			var user = GetById(id);

			user.ProfileImageurl = uri.AbsoluteUri;
			_context.Update(user);
		await  _context.SaveChangesAsync();
		}
	}
}
