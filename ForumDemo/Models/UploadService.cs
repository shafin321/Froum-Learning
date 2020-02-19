using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ForumDemo.Models
{
	public class UploadService : IUpload
	{
		public IConfiguration Configuration;
		public UploadService(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		public CloudBlobContainer GetBlobContainer(string connectionString)
		{
			var storageAccount = CloudStorageAccount.Parse(connectionString);
			var blobClient = storageAccount.CreateCloudBlobClient();
			return blobClient.GetContainerReference("profile-images");
		}
	}
}
