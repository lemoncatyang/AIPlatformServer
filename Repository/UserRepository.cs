using System.Linq;
using DatabaseInitializer;
using Model;

namespace Repository
{
    public class UserRepository: Repository<ApplicationUser>, IUserRepository
    {
        public int GetIdentityNumber()
        {
            return !Context.ApplicationUsers.Any()? 0: Context.ApplicationUsers.Max(u => u.IdentityNumber) + 1;
        }

        public string GetUserNameBasedOnIdentityNumber(int identityNumber)
        {
            var firstOrDefault = Context.ApplicationUsers.FirstOrDefault(u => u.IdentityNumber == identityNumber);
            return firstOrDefault?.UserName;
        }


        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
