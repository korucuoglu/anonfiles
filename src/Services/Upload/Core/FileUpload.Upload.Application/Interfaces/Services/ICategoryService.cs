using FileUpload.Shared.Wrappers;
using FileUpload.Shared.Dtos.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Features.Commands.Categories;

namespace FileUpload.Upload.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<Response<List<GetCategoryDto>>> GetAllAsync();
        public Task<Response<GetCategoryDto>> GetByIdAsync(int id);

        public Task<Response<GetCategoryDto>> AddAsync(AddCategoryCommand dto);
        public Task<Response<bool>> UpdateAsync(UpdateCategoryCommand dto);
        public Task<Response<bool>> DeleteByIdAsync(int id);
    }
}
