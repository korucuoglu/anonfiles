using AutoMapper;
using FileUpload.Shared.Services;
using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Redis;
using FileUpload.Upload.Application.Interfaces.Repositories.Dapper;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FileUpload.Upload.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Categories
{
    public class UpdateCategoryCommand : IRequest<Response<NoContent>>
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title boş olamaz");
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id boş olamaz");
        }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response<NoContent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHashService _hashService;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IRedisService redisService, IMapper mapper, ICategoryRepository categoryRepository, IHashService hashService)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _hashService = hashService;
        }

        public async Task<Response<NoContent>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);

            _unitOfWork.CategoryWriteRepository().Update(category);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<NoContent>.Fail(error: "Hata meydana geldi", statusCode: 500);
            }
            await _redisService.SetAsync($"categories-{category.Id}", category);

            return Response<NoContent>.Success(200);
        }
    }
}
