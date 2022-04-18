using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Persistence.Context;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class CategoryWriteRepository : WriteRepository<Domain.Entities.Category>, ICategoryWriteRepository
    {
        public CategoryWriteRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
