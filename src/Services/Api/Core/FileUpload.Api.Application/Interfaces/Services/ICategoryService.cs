using FileUpload.Api.Application.Dtos.Categories;
using FileUpload.Api.Application.Features.Commands.Categories.Add;
using FileUpload.Api.Application.Features.Commands.Categories.Update;
using FileUpload.Api.Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Interfaces.Services
{
    public interface ICategoryService
    {
        public Task<Response<List<GetCategoryDto>>> GetAllAsync();
        public Task<Response<GetCategoryDto>> GetByIdAsync(Guid id);

        public Task<Response<GetCategoryDto>> AddAsync(AddCategoryCommand dto);
        public Task<Response<bool>> UpdateAsync(UpdateCategoryCommand dto);
        public Task<Response<bool>> DeleteByIdAsync(Guid id);
    }
}
