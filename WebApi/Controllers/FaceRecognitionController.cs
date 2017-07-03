using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlobStorage;
using FaceRecognition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
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
        //[HttpPost("DeletePhoto")]
        //public async Task<IActionResult> DeletePhoto()
        //{
            
        //}
        //[HttpPost("RecognitionTest")]
        //public async Task<IActionResult> RecognitionTest(IFormFile file)
        //{
        //}

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
                GuidName = Guid.NewGuid()
            };

            UnitOfWork.ApplicationDbContext.UserFiles.Add(userFile);
            UnitOfWork.ApplicationDbContext.SaveChanges();

            var blockBlob = container.GetBlockBlobReference(userFile.GuidName.ToString());
            blockBlob.Properties.ContentType = file.ContentType;
            using (var fileStream = file.OpenReadStream())
            {
                var fileBitmap = new Bitmap(fileStream);
                fileBitmap = GrayScaleHelper.ToGrayImage(fileBitmap);
                //await blockBlob.UploadFromStreamAsync(fileBitmap);
                //await blockBlob.UploadFromFileAsync
            }
            return Ok();
        }

        public FaceRecognitionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<BlobStorageConfiguration> config) : base(unitOfWork, userManager, roleManager, config)
        {
        }
    }
}
