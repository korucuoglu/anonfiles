using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Features.Commands.Categories.Add;
using FileUpload.Application.Features.Commands.Categories.Delete;
using FileUpload.Application.Features.Commands.Categories.Update;
using FileUpload.Application.Features.Queries.Categories.GetAll;
using FileUpload.Application.Features.Queries.Categories.GetById;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileUpload.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMediator _mediator;

        public CategoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response<bool>> AddAsync(AddCategoryCommand dto)
        {
            return await _mediator.Send(dto);
        }

        public async Task<Response<bool>> DeleteByIdAsync(Guid id)
        {
            var query = new DeleteCategoryCommand() { Id = id };

            return await _mediator.Send(query);
        }

        public async Task<Response<List<GetCategoryDto>>> GetAllAsync()
        {
            var query = new GetAllCategoriesQueryRequest();

            return await _mediator.Send(query);
        }

        public async Task<Response<GetCategoryDto>> GetByIdAsync(Guid id)
        {
            var query = new GetCategoryByIdQueryRequest() { Id = id };

            return await _mediator.Send(query);
        }

        public async Task<Response<bool>> UpdateAsync(UpdateCategoryCommand dto)
        {
            return await _mediator.Send(dto);
        }
    }
}
