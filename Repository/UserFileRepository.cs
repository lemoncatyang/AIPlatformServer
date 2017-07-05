using System.Collections.Generic;
using System.Linq;
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
            return Context.UserFiles.Where(u => u.UserId == id).Select(u => u.GuidName.ToString()).ToList();
        }

        public List<UserFile> GetUserAllFiles(string id)
        {
            return Context.UserFiles.Where(u => u.UserId == id).ToList();
        }

        public UserFile GetUserFileBasedOnUserIdAndGuidName(string guidName)
        {
            return Context.UserFiles.FirstOrDefault(u => u.GuidName.ToString() == guidName);
        }
    }
}
