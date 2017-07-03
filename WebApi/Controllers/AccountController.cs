

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System.Threading.Tasks;
using BlobStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : BaseApiController
    {


        [HttpGet("register")]
        [AllowAnonymous]
        public async Task Register()
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                PhoneNumber = "123456789",
            };
            var result = await UserMananger.CreateAsync(admin, "abc@1234");

            var role = new ApplicationRole
            {
                Name = "Administrator"
            };

            result = await RoleManager.CreateAsync(role);
        }

        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) : base(unitOfWork, userManager, roleManager)
        {
        }
    }
}
