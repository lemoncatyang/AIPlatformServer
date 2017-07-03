using System;
using System.Collections.Generic;
using System.Text;
using DatabaseInitializer;

namespace Repository
{
    public interface IUnitOfWork
    {
        ApplicationDbContext ApplicationDbContext { get; set; }
    }
}
