using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace DatabaseInitializer
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(app.ApplicationServices.GetRequiredService<ApplicationDbContext>()), null, null, null, null, null, null, null, null);
            var roleManger = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(app.ApplicationServices.GetRequiredService<ApplicationDbContext>()), null, null, null, null, null);
            if (!context.ApplicationUsers.Any())
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
                var result1 = userManager.CreateAsync(admin, "yang03292").GetAwaiter().GetResult();

                var result2 = roleManger.CreateAsync(adminRole).Result;

                var result3 = userManager.AddToRoleAsync(admin, adminRole.Name).Result;
            }
        }
    }
}
