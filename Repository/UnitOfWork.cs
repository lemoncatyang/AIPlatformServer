using DatabaseInitializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext ApplicationDbContext{ get; set; }

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }
    }
}
