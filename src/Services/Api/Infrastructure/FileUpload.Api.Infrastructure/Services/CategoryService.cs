using FileUpload.Api.Application.Dtos.Categories;
using FileUpload.Api.Application.Features.Commands.Categories.Add;
using FileUpload.Api.Application.Features.Commands.Categories.Delete;
using FileUpload.Api.Application.Features.Commands.Categories.Update;
using FileUpload.Api.Application.Features.Queries.Categories.GetAll;
using FileUpload.Api.Application.Features.Queries.Categories.GetById;
using FileUpload.Api.Application.Interfaces.Services;
using FileUpload.Api.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Api.Infrastructure.Services
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

        public async Task<Response<GetCategoryDto>> GetByIdAsync(string id)
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
            dto.UserId = _sharedIdentityService.GetUserId;
            return await _mediator.Send(dto);
        }

        public async Task<Response<GetCategoryDto>> AddAsync(AddCategoryCommand dto)
        {
            dto.UserId = _sharedIdentityService.GetUserId;
            return await _mediator.Send(dto);
        }

        public async Task<Response<bool>> DeleteByIdAsync(string id)
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
