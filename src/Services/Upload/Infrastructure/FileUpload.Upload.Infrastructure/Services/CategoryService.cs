using FileUpload.Upload.Application.Features.Commands.Categories;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Shared.Dtos.Categories;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Features.Queries.Categories;

namespace FileUpload.Upload.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;


        public CategoryService(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<List<GetCategoryDto>>> GetAllAsync()
        {
            GetAllCategoriesQueryRequest query = new()
            {
                 UserId = _sharedIdentityService.GetUserId
            };

            return await _mediator.Send(query);
        }

        public async Task<Response<GetCategoryDto>> GetByIdAsync(int id)
        {

            var query = new GetCategoryByIdQueryRequest()
            {
                Id = id,
                UserId = _sharedIdentityService.GetUserId
            };

            return await _mediator.Send(query);
        }


        public async Task<Response<bool>> UpdateAsync(UpdateCategoryCommand dto)
        {
            dto.ApplicationUserId = _sharedIdentityService.GetUserId;
            return await _mediator.Send(dto);
        }

        public async Task<Response<GetCategoryDto>> AddAsync(AddCategoryCommand dto)
        {
            dto.ApplicationUserId = _sharedIdentityService.GetUserId;
            return await _mediator.Send(dto);
        }

        public async Task<Response<bool>> DeleteByIdAsync(int id)
        {
            var query = new DeleteCategoryCommand()
            {
                Id = id,
                UserId = _sharedIdentityService.GetUserId
            };

            return await _mediator.Send(query);
        }
    }
}
