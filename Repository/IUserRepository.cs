﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Repository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        int GetIdentityNumber();

        string GetUserNameBasedOnIdentityNumber(int identityNumber);
    }
}
