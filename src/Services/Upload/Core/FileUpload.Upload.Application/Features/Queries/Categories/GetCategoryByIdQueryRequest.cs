using FileUpload.Upload.Application.Interfaces.Redis;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Shared.Wrappers;
using FileUpload.Shared.Dtos.Categories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Upload.Application.Interfaces.Services;
using AutoMapper;
using FileUpload.Upload.Application.Interfaces.Repositories.Dapper;

namespace FileUpload.Upload.Application.Features.Queries.Categories
{
    public class GetCategoryByIdQueryRequest : IRequest<Response<GetCategoryDto>>
    {
        public int Id { get; set; }
    }

    public class GetCategoryByIdQueryRequestHandler : IRequestHandler<GetCategoryByIdQueryRequest, Response<GetCategoryDto>>
    {
        private readonly IRedisService _redisService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryRequestHandler(IUnitOfWork unitOfWork, IRedisService redisService, ISharedIdentityService sharedIdentityService, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
            _sharedIdentityService = sharedIdentityService;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<GetCategoryDto>> Handle(GetCategoryByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _categoryRepository.GetById(request.Id);

            var mapperData = _mapper.Map<GetCategoryDto>(data);

            return Response<GetCategoryDto>.Success(mapperData, 200);

            //var redisData = await _redisService.GetAsync<GetCategoryDto>($"categories-{request.Id}");

            //if (redisData != null)
            //{
            //    return Response<GetCategoryDto>.Success(redisData, 200);
            //}

            //var data = _unitOfWork.ReadRepository<Category>().Where(x => x.ApplicationUserId == _sharedIdentityService.GetUserId && x.Id == request.Id, false);

            //var mapperData = await _mapper.ProjectTo<GetCategoryDto>(data).FirstOrDefaultAsync();

            //await _redisService.SetAsync($"categories-{request.Id}", mapperData);

            //return Response<GetCategoryDto>.Success(mapperData, 200);
        }
    }
}
