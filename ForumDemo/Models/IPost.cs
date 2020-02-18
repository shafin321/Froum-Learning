using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumDemo.Models
{
	public interface IPost

	{ 
		Post GetById(int id);

		IEnumerable<Post> GetAll();
		IEnumerable<Post> GetFilteredPosts(int id , string searchQuery);
		IEnumerable<Post> GetFilteredPosts(string searchQuery);

		IEnumerable<Post> GetLatestPost(int Count);

		Task Add(Post post);
		Post Create(Post post);
		Post Delete(int id);
		Task EditPostContent(int id, string newContent);

		Post EditContent(int id, string newContent);
		Post ReplyPost(PostReply reply);
		Task AddReply(PostReply reply);

		IEnumerable<Post> GetPostByForum(int id);

	}
}
