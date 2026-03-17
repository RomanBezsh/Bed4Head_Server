using Bed4Head.DAL.EF;
using Bed4Head.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bed4Head.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;
        private UserRepository _userRepository;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }

        public IRepository<User> Users => _userRepository ??= new UserRepository(_db);

        public async Task<int> CompleteAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
