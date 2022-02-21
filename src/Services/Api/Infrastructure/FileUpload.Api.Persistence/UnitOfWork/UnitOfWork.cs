using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Persistence.Context;
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
        public async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }
    }
}
