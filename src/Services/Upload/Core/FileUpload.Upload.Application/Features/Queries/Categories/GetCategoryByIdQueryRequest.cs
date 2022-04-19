using AutoMapper;
using FileUpload.Shared.Dtos.Categories;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Redis;
using FileUpload.Upload.Application.Interfaces.Repositories;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Queries.Categories
{
    public class GetCategoryByIdQueryRequest : IRequest<Response<GetCategoryDto>>
    {
        public int Id { get; set; }
    }

    public class GetCategoryByIdQueryRequestHandler : IRequestHandler<GetCategoryByIdQueryRequest, Response<GetCategoryDto>>
    {
        private readonly IRedisService _redisService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdQueryRequestHandler(IRedisService redisService, ISharedIdentityService sharedIdentityService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _redisService = redisService;
            _sharedIdentityService = sharedIdentityService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<GetCategoryDto>> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _redisService.IsKeyAsync($"categories-{request.Id}");

            if (result)
            {
                var redisData = await _redisService.GetAsync<GetCategoryDto>($"categories-{request.Id}");
                return Response<GetCategoryDto>.Success(redisData, 200);
            }

            var entity = _unitOfWork.CategoryReadRepository().Where(
                x => x.Id == request.Id &&
                x.UserId == _sharedIdentityService.GetUserId,
                tracking: false);

            var data = await _mapper.ProjectTo<GetCategoryDto>(entity).FirstOrDefaultAsync();

            await _redisService.SetAsync($"categories-{request.Id}", data);

            return Response<GetCategoryDto>.Success(data, 200);
        }
    }
}
