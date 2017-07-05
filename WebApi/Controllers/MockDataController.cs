using System.Linq;
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
    public class MockDataController : BaseApiController
    {


        [HttpGet("addUsers")]
        [AllowAnonymous]
        public async Task AddUsers()
        {
            if (!UnitOfWork.Users.Any())
            {
                var admin = new ApplicationUser
                {
                    UserName = "szhchengyang@163.com",
                    PhoneNumber = "18251165658",
                    Email = "szhchengyang@163.com"
                };

                var adminRole = new ApplicationRole
                {
                    Name = ApplicationRoleType.Administrator.ToString()
                };
                var result1 = await UserManager.CreateAsync(admin, "yang03292");

                var result2 = await RoleManager.CreateAsync(adminRole);

                var result3 = await UserManager.AddToRoleAsync(admin, adminRole.Name);
            }
        }

        public MockDataController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) : base(unitOfWork, userManager, roleManager)
        {
        }
    }
}
