using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlobStorage;
using FaceRecognition;
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
        public RecognitionResult FaceRecognition(IFormFile file)
        {
            var bitmaps = new List<Bitmap>();
            var storageAccount = CloudStorageAccount.Parse(BlobStorageConfiguration.ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var users = UnitOfWork.Users.GetAll().ToList();
            var labels = new List<int>();

            users.ForEach(u =>
            {
                var container = blobClient.GetContainerReference(u.Id);
                var filesInfo = UnitOfWork.UserFiles.GetUserAllFiles(u.Id);
                filesInfo.ForEach(f =>
                {
                    var blockBlob = container.GetBlockBlobReference(f.GuidName.ToString());
                    using (var memoryStream = new MemoryStream())
                    {
                        blockBlob.DownloadToStream(memoryStream);
                        // var file =  File(memoryStream.GetBuffer(), f.ContentType);
                        bitmaps.Add(new Bitmap(memoryStream));
                    }
                    labels.Add(u.IdentityNumber);
                });
            });


            //// 处理用户上传的测试数据
            using (var fileStream = file.OpenReadStream())
            {
                var testBitmap = new Bitmap(fileStream);
                var result = FaceRecognitionProcessor.FaceRecognition(bitmaps, testBitmap, labels.ToArray());
                result.PredictedUserName = UnitOfWork.Users.GetUserNameBasedOnIdentityNumber(result.Predicted);
                return result;
            }
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

        public FaceRecognitionController(IUnitOfWork unitOfWork, IFaceRecognitionProcessor processor, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<BlobStorageConfiguration> config) : base(unitOfWork, processor, userManager, roleManager, config)
        {
        }
    }
}
