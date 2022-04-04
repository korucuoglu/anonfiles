using FileUpload.Upload.Application.Interfaces.Redis;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Domain.Entities;
using FileUpload.Shared.Dtos.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Mapping;

namespace FileUpload.Upload.Application.Features.Queries.Categories
{
    public class GetCategoryByIdQueryRequest : IRequest<Response<GetCategoryDto>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    public class GetCategoryByIdQueryRequestHandler : IRequestHandler<GetCategoryByIdQueryRequest, Response<GetCategoryDto>>
    {
        private readonly IRedisService _redisService;
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdQueryRequestHandler(IUnitOfWork unitOfWork, IRedisService redisService)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }

        public async Task<Response<GetCategoryDto>> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var redisData = await _redisService.GetAsync<GetCategoryDto>($"categories-{request.Id}");

            if (redisData != null)
            {
                return Response<GetCategoryDto>.Success(redisData, 200);
            }

            var data = await _unitOfWork.ReadRepository<Category>().Where(x => x.ApplicationUserId == request.UserId && x.Id == request.Id, false).FirstOrDefaultAsync();

            var mapperData = ObjectMapper.Mapper.Map<GetCategoryDto>(data);

            await _redisService.SetAsync($"categories-{data.Id}", mapperData);

            return Response<GetCategoryDto>.Success(mapperData, 200);
        }
    }
}
