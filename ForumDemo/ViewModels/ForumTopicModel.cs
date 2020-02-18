using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.ViewModels
{
	public class ForumTopicModel
	{
		public  ForumListingModel Forum { get; set; }
		public IEnumerable<PostListingModel> Posts { get; set; }
		public string SearchQuery { get; set; }
		public bool EmptySearchResults { get; set; }
	}
}
