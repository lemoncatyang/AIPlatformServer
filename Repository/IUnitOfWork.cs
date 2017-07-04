using System;
using System.Collections.Generic;
using System.Text;
using DatabaseInitializer;
using Model;

namespace Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();

        IUserFileRepository UserFiles { get; }

        IRepository<ApplicationUser> Users { get; }
    }
}
