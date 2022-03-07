using AutoMapper;
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

        public UpdateCategoryCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var writeRepository = _unitOfWork.WriteRepository<Category>();
            var readRepository = _unitOfWork.ReadRepository<Category>();

            if (!readRepository.Any(x => x.ApplicationUserId == request.ApplicationUserId && x.Id == new Guid(request.Id)))
            {
                return Response<bool>.Fail(false, 200);
            }

            var category = _mapper.Map<Category>(request);
            writeRepository.Update(category);

            bool result = await _unitOfWork.SaveChangesAsync() > 0;

            if (!result)
            {
                return Response<bool>.Fail(result, 200);
            }

            return Response<bool>.Success(result, 200);
        }
    }
}
