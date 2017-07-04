using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Repository
{
    public interface IUserFileRepository: IRepository<UserFile>
    {
        List<string> GetFileNameList(string id);

        UserFile GetUserFileBasedOnUserIdAndGuidName(string guidName);
    }
}
