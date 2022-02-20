using AutoMapper;
using FileUpload.Application.Dtos.Categories;
using FileUpload.Application.Interfaces.Repositories;
using FileUpload.Application.Interfaces.Services;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Queries.Categories.GetById
{
    public class GetCategoryByIdQueryRequest: IRequest<Response<GetCategoryDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetCategoryByIdQueryRequestHandler : IRequestHandler<GetCategoryByIdQueryRequest, Response<GetCategoryDto>>
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryRequestHandler(IRepository<Category> categoryRepository, IMapper mapper, ISharedIdentityService sharedIdentityService)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<GetCategoryDto>> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _categoryRepository.Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId && x.Id == request.Id).Select(x => new GetCategoryDto()
            {
                Id = x.Id,
                Title = x.Title
            }).FirstOrDefaultAsync();

            return Response<GetCategoryDto>.Success(data, 200);
        }
    }
}
