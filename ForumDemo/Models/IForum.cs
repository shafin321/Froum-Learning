using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Models
{
	public interface IForum
	{
		Forum GetById(int id);
		IEnumerable<Forum> GetAll();
		IEnumerable<ApplicationUser> GetAllUsers(); //GetAllActive User

		Forum Create(Forum forum);
		Forum Delete(int id);
		Forum UpdateForumDescription(int forumId, string newDescription);
		Forum UpdateForumTitle(int forumId, string newTitle);
	}
}
