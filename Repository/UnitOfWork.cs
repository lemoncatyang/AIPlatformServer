using DatabaseInitializer;
using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public IUserFileRepository UserFiles { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            UserFiles = new UserFileRepository(applicationDbContext);
            Users = new UserRepository(applicationDbContext);
        }

        public void Dispose()
        {
            _applicationDbContext?.Dispose();
        }

        public void Complete()
        {
            _applicationDbContext.SaveChanges();
        }
    }
}
