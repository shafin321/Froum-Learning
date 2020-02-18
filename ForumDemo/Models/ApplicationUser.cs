﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ForumDemo.Models
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public int Rating { get; set; }
		public string ProfileImageurl { get; set; }
		public DateTime MemberSince { get; set; }
		public bool IsActive { get; set; }
	}
}