using FileUpload.Shared.Wrappers;
using FileUpload.Upload.Application.Interfaces.Redis;
using FileUpload.Upload.Application.Interfaces.Repositories.Dapper;
using FileUpload.Upload.Application.Interfaces.Services;
using FileUpload.Upload.Application.Interfaces.UnitOfWork;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Upload.Application.Features.Commands.Categories
{
    public class DeleteCategoryCommand : IRequest<Response<NoContent>>
    {
        public int Id { get; set; }
    }

    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage($"Id boş olamaz");
        }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Response<NoContent>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisService _redisService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IRedisService redisService, ISharedIdentityService sharedIdentityService, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _redisService = redisService;
            _sharedIdentityService = sharedIdentityService;
            _categoryRepository = categoryRepository;
        }

        public async Task<Response<NoContent>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            bool result = await _categoryRepository.Delete(request.Id);

            if (!result)
            {
                return Response<NoContent>.Fail("Veri silinemedi", 500);
            }

            return Response<NoContent>.Success(200);


            //await _unitOfWork.WriteRepository<Category>().RemoveAsync(x => x.ApplicationUserId == _sharedIdentityService.GetUserId && x.Id == request.Id);

            //bool result = await _unitOfWork.SaveChangesAsync() > 0;

            //if (!result)
            //{
            //    return Response<NoContent>.Fail("Veri silinemedi", 500);
            //}

            //await _redisService.RemoveAsync($"categories-{request.Id}");

            //return Response<NoContent>.Success(200);
        }
    }
}
