using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Models
{
   public	interface IApplicationUser
	{
		ApplicationUser GetById(string id);
		IEnumerable<ApplicationUser> GetALL();
		Task SetProfileImage(string id, Uri uri);
		Task IncreamentRating(string id,Type type);
	}
}
