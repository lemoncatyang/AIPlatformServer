using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseInitializer;
using Model;

namespace Repository
{
    public class UserFileRepository : Repository<UserFile>, IUserFileRepository
    {
        public UserFileRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<string> GetFileNameList(string id)
        {
            return Context.UserFiles.OrderBy(u => u.UserId).Select(u => u.GuidName.ToString()).ToList();
        }

        public UserFile GetUserFileBasedOnUserIdAndGuidName(string guidName)
        {
            return Context.UserFiles.FirstOrDefault(u => u.GuidName.ToString() == guidName);
        }
    }
}
