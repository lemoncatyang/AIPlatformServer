using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlobStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Model;
using Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class FaceRecognitionController : BlobAiController
    {
        [HttpPost("FaceRecognition")]
        public async Task FaceRecognition(IFormFile file)
        {

            
        }

        [HttpGet("TraningBeforeTest")]
        public async Task TranningBeforeTest()
        {
            
        }

        [HttpGet("CurrentUserRelatedPhotosList")]
        public async Task<List<string>> GetCurrentUserRelatedPhotosList()
        {
            var currentUser = await GetCurrentUserAsync();
            return UnitOfWork.UserFiles.GetFileNameList(currentUser.Id);
        }

        [HttpGet("GetImage/{id}")]
        [AllowAnonymous]
        public FileContentResult GetImageFileByGuidName(string id)
        {
            var storageAccount = CloudStorageAccount.Parse(BlobStorageConfiguration.ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var file = UnitOfWork.UserFiles.GetUserFileBasedOnUserIdAndGuidName(id);
            if (file == null)
                return null;
            var container = blobClient.GetContainerReference(file.UserId);
            var blockBlob = container.GetBlockBlobReference(id);
            var memoryStream = new MemoryStream();
            blockBlob.DownloadToStream(memoryStream);
            return File(memoryStream.GetBuffer(), file.ContentType);
        }

        [HttpPost("UploadNewPhoto")]
        public async Task<IActionResult> UploadNewPhoto(IFormFile file)
        {
            var storageAccount = CloudStorageAccount.Parse(BlobStorageConfiguration.ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var currentUser = await GetCurrentUserAsync();
            var container = blobClient.GetContainerReference(currentUser.Id);
            await container.CreateIfNotExistsAsync();

            var userFile = new UserFile
            {
                Id = Guid.NewGuid(),
                OriginalFileName = file.FileName,
                GuidName = Guid.NewGuid(),
                UserId = currentUser.Id,
                ContentType = file.ContentType
            };

            UnitOfWork.UserFiles.Add(userFile);
            UnitOfWork.Complete();

            var blockBlob = container.GetBlockBlobReference(userFile.GuidName.ToString());
            blockBlob.Properties.ContentType = userFile.ContentType;
            using (var fileStream = file.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
            return Ok();
        }

        public FaceRecognitionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<BlobStorageConfiguration> config) : base(unitOfWork, userManager, roleManager, config)
        {
        }
    }
}
