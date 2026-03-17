using Bed4Head.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bed4Head.DAL.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }

        Task<int> CompleteAsync();

    }
}
