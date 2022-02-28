using FileUpload.Api.Application.Interfaces.Settings;
using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Interfaces.Repositories;
using FileUpload.Api.Domain.Common;
using FileUpload.Api.Persistence.Repositories;

namespace FileUpload.Api.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly IDatabaseSettings _databaseSettings;

        public UnitOfWork(IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new Repository<T>(_databaseSettings);
        }

       
    }
}
