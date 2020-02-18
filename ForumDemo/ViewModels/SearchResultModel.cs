using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.ViewModels
{
	public class SearchResultModel
	{
		public IEnumerable<PostListingModel> Posts { get; set; }
		public string  SearchQuery { get; set; }
		public bool  EmptySearchResult { get; set; }


	}
}
