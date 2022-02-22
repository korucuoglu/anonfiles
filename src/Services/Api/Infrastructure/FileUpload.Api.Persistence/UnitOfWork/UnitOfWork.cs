using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Persistence.Context;
using FileUpload.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace FileUpload.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }
        public async ValueTask DisposeAsync() => await _context.DisposeAsync();

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
