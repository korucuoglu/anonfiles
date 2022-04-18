using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Persistence.Context;

namespace FileUpload.Upload.Persistence.Repositories
{
    public class CategoryReadRepository : ReadRepository<Domain.Entities.Category>, ICategoryReadRepository
    {
        public CategoryReadRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
