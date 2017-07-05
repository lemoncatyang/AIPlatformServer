using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlobStorage;
using FaceRecognition;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    public class BlobAiController : Controller
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        protected UserManager<ApplicationUser> UserMananger;

        protected RoleManager<ApplicationRole> RoleManager;

        protected BlobStorageConfiguration BlobStorageConfiguration;

        protected IFaceRecognitionProcessor FaceRecognitionProcessor { get; set; }

        public BlobAiController(IUnitOfWork unitOfWork, IFaceRecognitionProcessor faceRecognitionProcessor, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<BlobStorageConfiguration> config)
        {
            UnitOfWork = unitOfWork;
            UserMananger = userManager;
            RoleManager = roleManager;
            BlobStorageConfiguration = config.Value;
            FaceRecognitionProcessor = faceRecognitionProcessor;
        }

        protected async Task<ApplicationUser> GetCurrentUserAsync() => await UserMananger.GetUserAsync(HttpContext.User);
    }
}
