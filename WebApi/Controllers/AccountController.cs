using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : BaseApiController
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task Register(CreateUserModel userModel)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    UserName = userModel.UserName,
                    IdentityNumber = UnitOfWork.Users.GetIdentityNumber(),
                    Name = userModel.NickName
                };

                var result = await UserManager.CreateAsync(user, userModel.Password);

                var role = await RoleManager.FindByNameAsync("User");
                if (role == null)
                {
                    role = new ApplicationRole
                    {
                        Name = "User"
                    };
                    result = await RoleManager.CreateAsync(role);
                }

                result = await UserManager.AddToRoleAsync(user, role.Name);
            }
        }

        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager) : base(unitOfWork, userManager, roleManager)
        {
        }
    }
}
