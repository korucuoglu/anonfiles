using AutoMapper;
using FileUpload.Api.Application.Interfaces.Redis;
using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Categories.Update
{
    public class UpdateCategoryCommand : IRequest<Response<bool>>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Guid ApplicationUserId { get; set; }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id boş olamaz");
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title boş olamaz");
        }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;

        public UpdateCategoryCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IRedisService redisService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _redisService = redisService;
        }

        public async Task<Response<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {

            var category = _mapper.Map<Category>(request);
            _unitOfWork.WriteRepository<Category>().Update(category);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<bool>.Fail(result, 200);
            }

            await _redisService.SetAsync($"categories-{request.Id}", category);

            return Response<bool>.Success(result, 200);
        }
    }
}
