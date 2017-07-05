

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System.Threading.Tasks;
using BlobStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Repository;

namespace WebApi.Controllers
{
    public class BaseApiController : Controller
    {
        protected IUnitOfWork UnitOfWork { get; set; }

        protected UserManager<ApplicationUser> UserManager;

        protected RoleManager<ApplicationRole> RoleManager;

        public BaseApiController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            UnitOfWork = unitOfWork;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        protected async Task<ApplicationUser> GetCurrentUserAsync() => await UserManager.GetUserAsync(HttpContext.User);
    }
}
