using FileUpload.Api.Application.Interfaces.UnitOfWork;
using FileUpload.Api.Application.Wrappers;
using FileUpload.Api.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Api.Application.Features.Commands.Categories.Delete
{
    public class DeleteCategoryCommand : IRequest<Response<bool>>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
    }

    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage($"Id boş olamaz");
        }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Category>();
            
            var category = await repository.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.Id == request.Id);

            await repository.Remove(category);

            return Response<bool>.Success(true, 200);
        }
    }
}
