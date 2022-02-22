using FileUpload.Application.Interfaces.UnitOfWork;
using FileUpload.Application.Wrappers;
using FileUpload.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileUpload.Application.Features.Commands.Categories.Delete
{
    public class DeleteCategoryCommand : IRequest<Response<bool>>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
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
            
            var category = await repository.FirstOrDefaultAsync(x => x.ApplicationUserId == request.UserId && x.Id == request.Id);

            repository.Remove(category);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<bool>.Fail(result, 200);

            }

            return Response<bool>.Success(result, 200);
        }
    }
}
