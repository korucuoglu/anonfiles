using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Upload.Domain.Common;
using FileUpload.Upload.Persistence.Context;
using FileUpload.Upload.Persistence.Repositories;
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
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public IReadRepository<T> ReadRepository<T>() where T: BaseEntity => new ReadRepository<T>(_context);
        public IFileReadRepository FileReadRepository() => new FileReadRepository(_context);
        public IFileWriteRepository FileWriteRepository() => new FileWriteRepository(_context);
        public IUserInfoWriteRepository UserInfoWriteRepository() => new UserInfoWriteRepository(_context);
        public IUserInfoReadRepository UserInfoReadRepository() => new UserInfoReadRepository(_context);
        public ICategoryWriteRepository CategoryWriteRepository() => new CategoryWriteRepository(_context);
        public ICategoryReadRepository CategoryReadRepository() => new CategoryReadRepository(_context);

        public async ValueTask DisposeAsync() => await _context.DisposeAsync();

    }
}
