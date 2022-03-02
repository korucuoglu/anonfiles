using FileUpload.Api.Application.Interfaces.Settings;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Interfaces.Repositories;
using FileUpload.Api.Domain.Common;
using FileUpload.Api.Persistence.Repositories;
using FileUpload.Api.Application.Interfaces.Context;
using System.Threading.Tasks;

namespace FileUpload.Api.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly IDatabaseSettings _databaseSettings;
        private readonly IMongoContext _context;

        public UnitOfWork(IDatabaseSettings databaseSettings, IMongoContext context)
        {
            _databaseSettings = databaseSettings;
            _context = context;
        }

        public async Task<bool> Commit()
        {
            return await _context.SaveChanges() > 0;
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new Repository<T>(_databaseSettings, _context);
        }

       
    }
}
