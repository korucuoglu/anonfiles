using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Persistence.Repositories;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Upload.Domain.Common;
using FileUpload.Upload.Persistence.Context;
using System.Threading.Tasks;

namespace FileUpload.Upload.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IReadRepository<T> ReadRepository<T>() where T : BaseEntity
         => new ReadRepository<T>(_context);

        public IWriteRepository<T> WriteRepository<T>() where T : BaseEntity
            => new WriteRepository<T>(_context);
       
        public Task<int> SaveChangesAsync()
         => _context.SaveChangesAsync();

        public async ValueTask DisposeAsync() => await _context.DisposeAsync();
    }
}
