using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.ViewModels
{
	public class HomeIndexModel
	{
		public IEnumerable<PostListingModel> LatestPost { get; set; }
		public string SearchQuery { get; set; }

	}
}
