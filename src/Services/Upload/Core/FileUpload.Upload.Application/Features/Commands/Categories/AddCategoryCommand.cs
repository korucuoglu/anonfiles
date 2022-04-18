using AutoMapper;
using FileUpload.Shared.Dtos.Categories;
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
    public class AddCategoryCommand : IRequest<Response<int>>
    {
        public string Title { get; set; }
    }
    public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title boş olamaz");
        }
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Response<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;

        public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IRedisService redisService, IMapper mapper, IHashService hashService)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
            _mapper = mapper;
            _hashService = hashService;
        }

        public async Task<Response<int>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            await _unitOfWork.CategoryWriteRepository().AddAsync(category);
            var result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<int>.Fail("Kayıt sırasında hata meydana geldi");
            }

            var dto = _mapper.Map<GetCategoryDto>(category);
            await _redisService.SetAsync($"categories-{category.Id}", dto);

            return Response<int>.Success(_hashService.Encode(category.Id), 201);

        }
    }
}
